using UnityEngine;

public class RotateCube : MonoBehaviour
{
    /* ==== Inspector-accessible variables ==== */
    [SerializeField]
    private Transform rubiksTransform = null;

    [SerializeField]
    [Tooltip("Angular velocity or the cube's rotation")]
    [Range(1f, 10f)]
    private float rotSpeed = .01f;

    [SerializeField]
    [Tooltip("Strength of the spherical interpolation between 2 rotations")]
    [Range(0f, 1f)]
    private float slerpStrength = .175f;

    /* ==== Internal variables ==== */
    private Transform   camTransform = null;
    private Quaternion  targetRot;
    private Vector3     rotAxis;
    public  bool        allowFullCubeRotation = true;

    /* ==== Methods ==== */
    void Start()
    {
        SavedData save = GameManager.GetSave();

        if (save != null)
            targetRot = new Quaternion(save.rubiksPosRot.rot[0], save.rubiksPosRot.rot[1], save.rubiksPosRot.rot[2], save.rubiksPosRot.rot[3]);
        else
            targetRot = transform.rotation;

        camTransform = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetMouseButton(1) && allowFullCubeRotation)
        {
            // Define an axis in the plane of the camera
            rotAxis = Input.GetAxis("Mouse X") * camTransform.right + Input.GetAxis("Mouse Y") * camTransform.up;

            // Rotate it by 90° in the plane of the camera, to make the cube rotate in the same direction as the mouse
            rotAxis.Set(rotAxis.y, -rotAxis.x, rotAxis.z);

            // Express this axis into the cube's referential
            rotAxis.Set(Vector3.Dot(rotAxis, rubiksTransform.right),
                        Vector3.Dot(rotAxis, rubiksTransform.up),
                        Vector3.Dot(rotAxis, rubiksTransform.forward));
            
            // Apply the rotation, taking into account the intensity of the last mouse movement
            targetRot *= Quaternion.AngleAxis(rotAxis.magnitude * rotSpeed, Vector3.Normalize(rotAxis));
        }

        rubiksTransform.rotation = Quaternion.Slerp(rubiksTransform.rotation, targetRot, slerpStrength);
    }
}
