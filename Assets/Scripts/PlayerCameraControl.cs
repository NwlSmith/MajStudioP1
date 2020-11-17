using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControl : MonoBehaviour
{

    public bool hideMouse;

    Camera cam;

    public GameObject chair;

    Vector2 mouseAxis;
    public float mouseSensitivity;
    float camX;

    bool shootRay;

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
        if(shootRay && CardManager.instance.canPressButtons)
        {
            ButtonRay();
        }
    }

    void MouseInputs()
    {
        mouseAxis = new Vector2(Input.GetAxis("Mouse X") * mouseSensitivity, Input.GetAxis("Mouse Y") * mouseSensitivity);

        if(Input.GetMouseButtonDown(0))
        {
            shootRay = true;
        }
    }

    void MouseMovements()
    {
        transform.Rotate(Vector3.up * mouseAxis.x);

        camX -= mouseAxis.y;
        camX = Mathf.Clamp(camX, -90, 90);

        cam.transform.localRotation = Quaternion.Euler(camX, 0, 0);

        chair.transform.rotation = Quaternion.Lerp(chair.transform.rotation, transform.rotation, 0.2f);
    }

    void ButtonRay()
    {
        Ray buttonRay = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(buttonRay, out hit, 10f))
        {
            Debug.DrawRay(buttonRay.origin, buttonRay.direction, Color.magenta);
            Debug.Log(hit.collider.gameObject);
            if(hit.collider.tag == "Button")
            {
                hit.collider.gameObject.GetComponent<ButtonPress>().Pressed();
            }
        }

        shootRay = false;
    }
}
