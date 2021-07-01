using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouseCameraControl : MonoBehaviour
{

    public bool hideMouse;

    Camera cam;

    public GameObject chair;

    Vector2 mouseAxis;
    public float mouseSensitivity;
    float camX;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();

        if(hideMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        
    }

    void Update()
    {
        MouseInputs();
        MouseMovements();
    }

    void MouseInputs()
    {
        float x = Mouse.current.delta.x.ReadValue() * mouseSensitivity;
        float y = Mouse.current.delta.y.ReadValue() * mouseSensitivity;
        mouseAxis = new Vector2(x, y);
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
