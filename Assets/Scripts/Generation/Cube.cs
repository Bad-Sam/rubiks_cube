using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private GameObject facePrefab;

    private GameObject[] faces;

    [SerializeField]
    float offsetScale = 5;

    private void Start()
    {
        Vector3[] offsets = new[]
        {   
            Vector3.up, Vector3.down, 
            Vector3.right, Vector3.left, 
            Vector3.forward, Vector3.back
        };

        faces = new GameObject[6];
        for (int i = 0; i < 6; i++)
        {
            faces[i] = Instantiate(facePrefab);
            Transform t = faces[i].transform;
            t.parent = this.transform;
            t.localPosition = offsets[i] * offsetScale;
            t.localRotation = Quaternion.LookRotation(-offsets[i]);
            t.Rotate(-90,0,0);
        }
    }
}
