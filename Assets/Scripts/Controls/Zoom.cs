using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    private Transform camTransform = null;

    void Start()
    {
        camTransform = Camera.main.transform;
    }

    void Update()
    {
        camTransform.position += Input.mouseScrollDelta.y * camTransform.forward;
    }
}
