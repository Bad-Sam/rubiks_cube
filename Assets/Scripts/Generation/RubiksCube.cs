using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RubiksCube : MonoBehaviour
{
    private Camera              cam         = null;
    private GameObject          cubePrefab  = null;
    private List<GameObject>    cubes       = new List<GameObject>();

    /* Number of sub cubes aligned in the Rubik's cube. */
    private int                 size        = 2;
    /* Half of the distance between the location of each sub cube. */
    private float               cubeOffset  = 0f;
    private float               initialCamZ = 0f;

    private BoxCollider boxCollider = null;

    void Awake()
    {
        cam         = Camera.main;
        cubePrefab  = Resources.Load<GameObject>("Cube");
        cubeOffset  = cubePrefab.GetComponent<Cube>().OffsetScale;
        initialCamZ = cam.transform.position.z;
        // TODO: search for a save
        // TODO: if a save is found, load the RubiksCube with its parameters

        boxCollider = GetComponent<BoxCollider>();
        UpdateView();
    }


    void Start()
    {
        //cubes = new Cube[size * size * size];
        GenerateCubes();
    }


    void Update()
    {
       
        
    }
    public void RotateFace(Plane p, float angle)
    {
        List<GameObject> face = GetCubesOnPlane(p, cubeOffset * 4);
        foreach (GameObject cube in face)
        {
            cube.transform.RotateAround(transform.position, p.normal, angle);
        }
    }

    public void RotateFace(Vector3 normal, float angle)
    {
        Plane p = new Plane(normal, cubeOffset * size);
        RotateFace(p, angle);
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
        foreach (GameObject cube in cubes)
        {
            Destroy(cube);
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

        // TODO: shuffle the Rubik's Cube
    }


    public void InitializeColors()
    {

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
                        cubes.Add(newObject);
                    }
                }
            }
        }
    }


    public List<GameObject> GetCubesOnPlane(Plane p, float delta = 0.1f)
    {
        List<GameObject> cubesOnPlane = new List<GameObject>();
        foreach (GameObject cube in cubes)
        {
            Vector3 v = cube.transform.position;
            if ((p.ClosestPointOnPlane(v) - v).sqrMagnitude < delta)
            {
                cubesOnPlane.Add(cube);
            }
        }

        return cubesOnPlane;
    }
}
