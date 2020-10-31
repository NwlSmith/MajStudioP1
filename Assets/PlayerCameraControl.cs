using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControl : MonoBehaviour
{

    Camera cam;

    public GameObject chair;

    [Header("Camera Nums")]
    public Vector2 mouseAxis;
    public float mouseSensitivity;
    public float camX;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        MouseInputs();
        MouseMovements();
    }

    void MouseInputs()
    {
        mouseAxis = new Vector2(Input.GetAxis("Mouse X") * mouseSensitivity, Input.GetAxis("Mouse Y") * mouseSensitivity);
    }

    void MouseMovements()
    {
        transform.Rotate(Vector3.up * mouseAxis.x);

        camX -= mouseAxis.y;
        camX = Mathf.Clamp(camX, -90, 90);

        cam.transform.localRotation = Quaternion.Euler(camX, 0, 0);

        chair.transform.rotation = Quaternion.Lerp(chair.transform.rotation, transform.rotation, 0.2f);
    }
}
