using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshFilter))]
public class Face : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    public Material GetMaterial()
    {
        return meshRenderer.material; 
    }

    public void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material mat)
    {
        meshRenderer.material = mat;
    }
}
