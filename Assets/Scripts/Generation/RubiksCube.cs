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

    private GameObject[] cubes; 



    // Start is called before the first frame update
    void Start()
    {
        cubes = new GameObject[size * size * size];
        GenerateCubes();

    }

    // Update is called once per frame
    void Update()
    {
        
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
                    GameObject newObject = Instantiate(cubePrefab, transform);
                    newObject.transform.localPosition = 2 * offsetScale * new Vector3(x, y, z) - halfSizeVec;
                    cubes[x + y * size + z * size * size] = newObject;
                    //cubes[x + y * size + z * size * size].transform.parent = this.transform;
                }
            }
        }
    }
}
