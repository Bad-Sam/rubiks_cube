﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider))]
public class RubiksCube : MonoBehaviour
{
    private Camera          cam         = null;
    private GameObject      cubePrefab  = null;
    private List<Cube>      cubes       = new List<Cube>();

    /* Number of sub cubes aligned in the Rubik's cube. */
    private int             size        = 2;

    /* Half of the distance between the location of each sub cube. */
    private float           cubeOffset  = 0f;

    private CubeSolved      solvedCondition = null;
    private RotateCube      cubeRotator = null;

    private BoxCollider     boxCollider = null;

    int nbMaxAnimatedShuffles = 10;

    private Coroutine animCoroutine = null;

    public bool isFaceRotating = false;

    void Awake()
    {
        cam             = Camera.main;
        cubePrefab      = Resources.Load<GameObject>("Cube");
        cubeOffset      = cubePrefab.GetComponent<Cube>().OffsetScale;
        solvedCondition = FindObjectOfType(typeof(CubeSolved)) as CubeSolved;

        boxCollider         = GetComponent<BoxCollider>();
        boxCollider.size    = new Vector3(size, size, size) * 10f;

    }


    public bool IsSolved() 
    {
        Debug.Assert(cubes.Count > 0);

        Cube firstObj = cubes[0].GetComponent<Cube>();

        foreach (Cube obj in cubes)
        {
            if (obj.transform.rotation != firstObj.transform.rotation)
            {
                return false;
            }
        }

        return true;
    }

    public IEnumerator RotateFaceAnimated(Plane rotationPlane, float totalAngle, float time = 0.2f, float delta = 0.03f)
    {
        cubeRotator.allowFullCubeRotation = false;
        isFaceRotating = true;

        float cumulatedRotation = 0f;
        for (float currentTime = 0f ; currentTime < time; currentTime += delta)
        {
            float angle = totalAngle / (time / delta);
            RotateFace(rotationPlane, angle);
            cumulatedRotation += angle;

            yield return new WaitForSeconds(delta);
        }

        RotateFace(rotationPlane, -cumulatedRotation);
        RotateFace(rotationPlane, totalAngle);
        OnFaceRotationEnd();

        cubeRotator.allowFullCubeRotation = true;
        isFaceRotating = false;
        yield return null;
    }

    public void OnFaceRotationEnd()
    {
        if (IsSolved())
        {
            solvedCondition.Trigger();
        }
    }

    void Start()
    {
        cubeRotator = FindObjectOfType(typeof(RotateCube)) as RotateCube;
        // SavedData is a serializable class defined in GameManager.cs
        SavedData save = GameManager.GetSave();

        if (save != null)
        {
            size = save.cubeSize;
            Vector3 pos     = new Vector3(save.rubiksPosRot.pos[0], save.rubiksPosRot.pos[1], save.rubiksPosRot.pos[2]);
            Quaternion rot  = new Quaternion(save.rubiksPosRot.rot[0],  save.rubiksPosRot.rot[1],  save.rubiksPosRot.rot[2], save.rubiksPosRot.rot[3]);

            transform.SetPositionAndRotation(pos, rot);
            GenerateCubes();

            uint i = 0u;

            foreach (Cube cube in cubes)
            {
                // PosRot is a subclass
                SavedData.PosRot posRotI = save.posRot[i++];

                pos = new Vector3(posRotI.pos[0], posRotI.pos[1], posRotI.pos[2]);
                rot = new Quaternion(posRotI.rot[0], posRotI.rot[1], posRotI.rot[2], posRotI.rot[3]);

                cube.transform.SetPositionAndRotation(pos, rot);
            }
        }

        else
            GenerateCubes();

        UpdateView();
    }

    public void RotateFace(Plane p, float angle)
    {
        List<GameObject> face = GetCubesOnPlane(p, cubeOffset * 4);
        foreach (GameObject cube in face)
        {
            Quaternion rot = Quaternion.AngleAxis(angle, p.normal);
            cube.transform.position = rot * (cube.transform.position - transform.position) + transform.position;
            cube.transform.rotation = rot * cube.transform.rotation;

            // We want the quaternion unit and with a positif w, 
            // so that a rotation  is only represented by one quaternion (for the win condition).
            cube.transform.rotation.Normalize();
            if (cube.transform.rotation.w < 0)
            {
                Quaternion q = cube.transform.rotation;
                cube.transform.rotation = new Quaternion(-q.x, -q.y, -q.z, -q.w);
            }
        }
    }

    // Adapt the camera position so the Rubik's Cube fits to the screen
    void UpdateView()
    {
        // Calculated from the camera's FOV, with a bit of trigonometry
        // The original formula is multiplicated by 2.5, so the view is "comfortable"
        float sign = Mathf.Sign(Vector3.Dot(cam.transform.position, Vector3.forward));
        cam.transform.position = new Vector3(0f, 0f, sign * Mathf.Sqrt(3) * size * cubeOffset * 1.25f / Mathf.Tan(.5f * Mathf.Deg2Rad * cam.fieldOfView));
    }


    public void Generate(int newSize, int shuffle)
    {
        // Remove current Rubik's Cube
        foreach (Cube cube in cubes)
        {
            Destroy(cube.gameObject);
        }

        cubes.Clear();

        // Update the size to the size of the new Rubik's Cube
        size = newSize;

        // Update camera view
        UpdateView();

        /* Updates collider size */
        boxCollider.size = new Vector3(size, size, size) * (cubeOffset * 2f);

        // Generate new Rubik'sCube
        GenerateCubes();

        if (animCoroutine != null)
        {
            StopCoroutine(animCoroutine);
        }

        // Shuffle the Rubik's Cube
        if (shuffle > nbMaxAnimatedShuffles)
        {
            for (int i = 0; i < shuffle - nbMaxAnimatedShuffles; i++)
            {
                Shuffle();
            }
            animCoroutine = StartCoroutine(AnimatedShuffle(nbMaxAnimatedShuffles));
        }
        else
        {
            animCoroutine = StartCoroutine(AnimatedShuffle(shuffle));
        }

        cubeRotator.allowFullCubeRotation = true;
    }

    private void GenerateCubes()
    {
        for (int z = 0; z < size; z++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (x == 0 || x == (size - 1) || y == 0 || y == (size - 1) || z == 0 || z == (size - 1))
                    {
                        GameObject newObject = Instantiate(cubePrefab, transform);
                        newObject.transform.localPosition = 2 * cubeOffset * new Vector3(x, y, z) - cubeOffset * (size - 1) * Vector3.one;
                        cubes.Add(newObject.GetComponent<Cube>());
                    }
                }
            }
        }
    }


    public List<GameObject> GetCubesOnPlane(Plane p, float delta = 0.1f)
    {
        List<GameObject> cubesOnPlane = new List<GameObject>();
        foreach (Cube cube in cubes)
        {
            Vector3 v = cube.transform.position;
            if ((p.ClosestPointOnPlane(v) - v).sqrMagnitude < delta)
            {
                cubesOnPlane.Add(cube.gameObject);
            }
        }

        return cubesOnPlane;
    }


    public Plane GetRandomShufflePlane()
    {
        Vector3[] axes =
        {
            transform.up,
            - transform.up,
            transform.right,
            - transform.right,
            transform.forward,
            - transform.forward
        };
        int randomAxisIndex = randomAxisIndex = UnityEngine.Random.Range(0, 5);
        Vector3 randomAxis = axes[randomAxisIndex];
        int randomLine = UnityEngine.Random.Range(-size, size);
        Vector3 point = transform.position + randomAxis * (cubeOffset * (randomLine - 0.5f));
        Plane p = new Plane(randomAxis, point);
        return p;
    }

    public void Shuffle()
    {
        Plane p = GetRandomShufflePlane();
        RotateFace(p, 90f);
    }

    IEnumerator AnimatedShuffle(int nbShuffles)
    {
        for (int i = 0; i < nbShuffles; i++)
        {
            float duration = 0.09f;
            Plane p = GetRandomShufflePlane();

            yield return RotateFaceAnimated(p, 90f, duration);
        }
    }


    public SavedData GetSavedData()
    {
        SavedData save = new SavedData(size, cubes.Count);

        uint i = 0u;
        foreach (Cube cube in cubes)
        {
            Vector3     pos = cube.transform.position;
            Quaternion  rot = cube.transform.rotation;

            SavedData.PosRot posRotI = save.posRot[i++];

            posRotI.pos[0] = pos.x;
            posRotI.pos[1] = pos.y;
            posRotI.pos[2] = pos.z;
            posRotI.rot[0] = rot.x;
            posRotI.rot[1] = rot.y;
            posRotI.rot[2] = rot.z;
            posRotI.rot[3] = rot.w;
        }

        save.rubiksPosRot.pos[0] = transform.position.x;
        save.rubiksPosRot.pos[1] = transform.position.y;
        save.rubiksPosRot.pos[2] = transform.position.z;
        save.rubiksPosRot.rot[0] = transform.rotation.x;
        save.rubiksPosRot.rot[1] = transform.rotation.y;
        save.rubiksPosRot.rot[2] = transform.rotation.z;
        save.rubiksPosRot.rot[3] = transform.rotation.w;

        return save;
    }
}