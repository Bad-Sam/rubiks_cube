using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RubiksCube : MonoBehaviour
{
    private GameObject          cubePrefab  = null;
    private List<GameObject>    cubes       = new List<GameObject>();
    /* Number of sub cubes aligned in the Rubik's cube. */
    private int                 size        = 3;
    /* Half of the distance between the location of each sub cube. */
    private float               offsetScale = 5f;

    private BoxCollider boxCollider = null;

    void Awake()
    {
        cubePrefab = Resources.Load<GameObject>("Cube");
        // TODO: search for a save
        // TODO: if a save is found, load the RubiksCube with its parameters


        boxCollider = GetComponent<BoxCollider>();
    }


    // Start is called before the first frame update
    void Start()
    {
        //cubes = new Cube[size * size * size];
        GenerateCubes();
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }
    public void RotateFace(Plane p, float angle)
    {
        List<GameObject> face = GetCubesOnPlane(p, offsetScale * 4);
        foreach (GameObject cube in face)
        {
            cube.transform.RotateAround(transform.position, p.normal, angle);
        }
    }

    public void RotateFace(Vector3 normal, float angle)
    {
        Plane p = new Plane(normal, offsetScale * size);
        RotateFace(p, angle);
    }

    public void Generate(int size_, int shuffle)
    {
        foreach (GameObject cube in cubes)
        {
            Destroy(cube);
        }

        cubes.Clear();

        size = size_;

        boxCollider.size = new Vector3(size, size, size) * (offsetScale * 2f);

        GenerateCubes();
        // TODO: shuffle the Rubik's Cube
    }


    public void InitializeColors()
    {
        
    }


    private void GenerateCubes()
    {
        Vector3 halfSizeVec = offsetScale * size * Vector3.one;
        for (int z = 0; z < size; z++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (x == 0 || x == (size - 1) || y == 0 || y == (size - 1) || z == 0 || z == (size - 1))
                    {
                        GameObject newObject = Instantiate(cubePrefab, transform);
                        //newObject.transform.localPosition = 2 * offsetScale * new Vector3(x, y, z) - Vector3.one * (offsetScale * 2);
                        newObject.transform.localPosition = 2 * offsetScale * new Vector3(x, y, z) - offsetScale * (size - 1) * Vector3.one;
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
