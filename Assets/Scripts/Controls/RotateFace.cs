using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFace : MonoBehaviour
{
    [SerializeField]
    private RubiksCube rubiks = null;

    private RotateCube cubeRotator = null;

    Plane rotationPlane;
    Vector3 firstHitPoint;
    Vector3 usedAxis = Vector3.up;

    int state = 0;

    float currentRotation = 0;
    float angularSpeed = 1f;//0.03f;
    int inputMode = 0;

    void Start()
    {
        cubeRotator = FindObjectOfType(typeof(RotateCube)) as RotateCube;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            switch (state)
            {
                case 0:
                    OnFirstClick();
                    break;
                case 1:
                    SelectAxis();
                    break;
                case 2:
                    OnDrag();
                    break;
            }
        }
        else
        {
            OnMouseRelease();
        }
    }

    void OnFirstClick()
    {
        cubeRotator.allowFullCubeRotation = false;

        Vector3 mouseLoc2D = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mouseLoc2D);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            rotationPlane = new Plane(hitInfo.normal, hitInfo.point);
            Debug.DrawRay(hitInfo.point, hitInfo.normal * 10000, Color.cyan, 9999);
            firstHitPoint = hitInfo.point;
            lastHitPoint = firstHitPoint;

            state = 1;
        }
    }

    Vector3 GetNearestAxis(Vector3 v, out float dotMax)
    {
        Vector3 returnedAxis = Vector3.zero;
        dotMax = 0;
        Vector3[] dirs = { rubiks.transform.right, rubiks.transform.up, rubiks.transform.forward };
        foreach (Vector3 axis in dirs)
        {
            float dot = Vector3.Dot(v, axis);
            if (Mathf.Abs(dot) > Mathf.Abs(dotMax))
            {
                returnedAxis = axis * Mathf.Sign(dot);
                dotMax = dot;
            }
        }
        return returnedAxis;
    }

    void SelectAxis()
    {
        Vector3 mouseLoc2D = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mouseLoc2D);
        rotationPlane.Raycast(ray, out float enter);
        Vector3 HitPointProj = ray.origin + enter * ray.direction;

        usedAxis = GetNearestAxis(HitPointProj - firstHitPoint, out float dotMax);
        float dotOffsetMin = 0.1F;
        if (Mathf.Abs(dotMax) > dotOffsetMin)
        {
            state = 2;
        }
    }

    Vector3 lastHitPoint;

    void OnDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        rotationPlane.Raycast(ray, out float enter);
        Vector3 hitPointProj = ray.origin + enter * ray.direction;

        Vector3 cross = Vector3.Cross(rotationPlane.normal, usedAxis.normalized);
        float dot = Vector3.Dot(hitPointProj - lastHitPoint, usedAxis);
        Plane p = new Plane(cross, firstHitPoint);
        rubiks.RotateFace(p, dot * angularSpeed);
        currentRotation += dot * angularSpeed;

        if (inputMode == 0)
            lastHitPoint = hitPointProj;
    }

    void OnMouseRelease()
    {
        if (state == 0)
            return;

        /* Set the angle of the face to a multiple of 90 degrees. */
        currentRotation %= 90;
        Vector3 cross = Vector3.Cross(rotationPlane.normal, usedAxis.normalized);
        Plane p = new Plane(cross, firstHitPoint);
        if (currentRotation < 45)
        {
            StartCoroutine(rubiks.RotateFaceAnimated(p, -currentRotation));
        }
        else
        {
            StartCoroutine(rubiks.RotateFaceAnimated(p, 90 - currentRotation));
        }
        currentRotation = 0;

        state = 0;
    }

}
