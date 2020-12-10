using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksCube : MonoBehaviour
{
    [SerializeField]
    private int size = 2;

    [SerializeField]
    float offsetScale = 5;

    [SerializeField]
    private GameObject cubePrefab;

    //private Cube[] cubes;
    private List<Cube> cubes = new List<Cube>();



    // Start is called before the first frame update
    void Start()
    {
        //cubes = new Cube[size * size * size];
        GenerateCubes();
    }

    // Update is called once per frame
    void Update()
    {
        Plane p = new Plane(Vector3.forward, offsetScale * size);
        List<Cube> face = GetCubesOnPlane(p, 1);
        foreach (Cube cube in face)
        {
            cube.transform.RotateAround(Vector3.zero, Vector3.forward, 1);
        }
    }

    public void InitializeColors()
    {
        
    }

    public void GenerateCubes()
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
                        newObject.transform.localPosition = 2 * offsetScale * new Vector3(x, y, z) - halfSizeVec;
                        //cubes[x + y * size + z * size * size] = newObject.GetComponent<Cube>();
                        //cubes[x + y * size + z * size * size].transform.parent = this.transform;
                        cubes.Add(newObject.GetComponent<Cube>());
                    }
                }
            }
        }
    }

    public List<Cube> GetCubesOnPlane(Plane p, float delta = 0.1f)
    {
        List<Cube> cubesOnPlane = new List<Cube>();
        foreach (Cube cube in cubes)
        {
            Vector3 v = cube.transform.position;
            //Debug.Log((p.ClosestPointOnPlane(v) - v).sqrMagnitude);
            //Debug.Log("point : " + v);
            //Debug.Log("closest : " + p.ClosestPointOnPlane(v));
            if ((p.ClosestPointOnPlane(v) - v).sqrMagnitude < delta)
            {
                cubesOnPlane.Add(cube);
            }
        }

        return cubesOnPlane;
    }
}
