using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFace : MonoBehaviour
{
    [SerializeField]
    private RubiksCube rubiks = null;

    Plane rotationPlane;
    Vector3 firstHitPoint;
    Vector3 usedAxis = Vector3.up;

    int state = 0;

    float currentRotation = 0;
    float angularSpeed = 0.03f;

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
        Vector3 mouseLoc2D = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mouseLoc2D);
        RaycastHit hitInfo;
        //Debug.DrawRay(ray.origin, ray.direction * 100, UnityEngine.Color.red, 10F);
        if (Physics.Raycast(ray, out hitInfo))
        {
            rotationPlane = new Plane(hitInfo.normal, hitInfo.point);
            firstHitPoint = hitInfo.point;

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
        float enter;
        rotationPlane.Raycast(ray, out enter);
        Vector3 HitPointProj = ray.origin + enter * ray.direction;

        float dotMax;
        usedAxis = GetNearestAxis(HitPointProj - firstHitPoint, out dotMax);
        float dotOffsetMin = 0.1F;
        if (Mathf.Abs(dotMax) > dotOffsetMin)
        {
            state = 2;
        }
    }

    void OnDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter;
        rotationPlane.Raycast(ray, out enter);
        Vector3 hitPointProj = ray.origin + enter * ray.direction;

        Vector3 cross = Vector3.Cross(rotationPlane.normal, usedAxis.normalized);
        float dot = Vector3.Dot(hitPointProj - firstHitPoint, usedAxis);
        Plane p = new Plane(cross, firstHitPoint);
        rubiks.RotateFace(p, dot * angularSpeed);// dotMax * 10);
        currentRotation += dot * angularSpeed;
    }

    void OnMouseRelease()
    {
        if (state == 0)
            return;

        /* Set the angle of the face to a multiple of 90 degrees. */
        // TODO : Interpolate
        currentRotation %= 90;
        Vector3 cross = Vector3.Cross(rotationPlane.normal, usedAxis.normalized);
        Plane p = new Plane(cross, firstHitPoint);
        if (currentRotation < 45)
        {
            //rubiks.RotateFace(p, -currentRotation);
            StartCoroutine(rubiks.EndRotation(p, -currentRotation));
        }
        else
        {
            //rubiks.RotateFace(p, 90 - currentRotation);
            StartCoroutine(rubiks.EndRotation(p, 90 - currentRotation));
        }
        currentRotation = 0;

        state = 0;
        //rubiks.OnFaceRotationEnd();
    }

}
