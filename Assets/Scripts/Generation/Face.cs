using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshFilter))]
public class Face : MonoBehaviour
{
    private Color color;

    private MeshRenderer meshRenderer;

    //[SerializeField]
    //private MeshFilter meshFilter = null;
    //[SerializeField]
    //private MeshRenderer meshRenderer = null;


    //public void SetColor(Color c)
    //{

    //}

    public void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material mat)
    {
        meshRenderer.material = mat;
    }
}
