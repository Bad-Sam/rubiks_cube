using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private GameObject facePrefab = null;

    private Face[] faces = null;

    [SerializeField]
    private Material[] materials = null;

    [SerializeField]
    private float offsetScale = 5;
    public float OffsetScale
    { get => offsetScale; private set => offsetScale = value; }

    public Face[] Faces
    { get => faces; }

    private void Awake()
    {
        Assert.IsNotNull(materials);
        foreach (Material mat in materials)
        {
            Assert.IsNotNull(mat);
        }
    }

    private void Start()
    {
        Vector3[] offsets = new[]
        {
            Vector3.up, Vector3.down,
            Vector3.right, Vector3.left,
            Vector3.forward, Vector3.back
        };

        faces = new Face[6];
        Vector3 halfSizeVec = Vector3.one * offsetScale;
        for (int i = 0; i < 6; i++)
        {
            faces[i] = Instantiate(facePrefab).GetComponent<Face>();
            Transform t = faces[i].transform;
            t.parent = this.transform;
            t.localPosition = offsets[i] * offsetScale;
            t.localRotation = Quaternion.LookRotation(-offsets[i]);
            t.Rotate(-90, 0, 0);


            Face face = faces[i].GetComponent<Face>();
            if (face != null)
            {
                face.SetMaterial(materials[i]);
            }
        }
    }
}
