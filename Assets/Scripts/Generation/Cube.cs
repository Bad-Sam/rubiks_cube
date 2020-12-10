using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private GameObject facePrefab;

    private GameObject[] faces;

    [SerializeField]
    private float offsetScale = 5;

    private void Start()
    {
        Vector3[] offsets = new[]
        {   
            Vector3.up, Vector3.down, 
            Vector3.right, Vector3.left, 
            Vector3.forward, Vector3.back
        };

        faces = new GameObject[6];
        Vector3 halfSizeVec = Vector3.one * offsetScale;
        for (int i = 0; i < 6; i++)
        {
            faces[i] = Instantiate(facePrefab);
            Transform t = faces[i].transform;
            t.parent = this.transform;
            t.localPosition = offsets[i] * offsetScale + halfSizeVec;
            t.localRotation = Quaternion.LookRotation(-offsets[i]);
            t.Rotate(-90,0,0);

            //Debug.Log(Vector3.Dot(faces[i].transform.position, faces[i].transform.position + offsets[i]));
            //if (Vector3.Dot(faces[i].transform.position, offsets[i]) < 0)
            //    Destroy(faces[i]);
        }
    }
}
