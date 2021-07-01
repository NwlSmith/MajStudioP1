using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonPresser : MonoBehaviour
{

    Camera _cam;
    bool _shootRay;
    
    private delegate void InputUpdateFunc();

    private InputUpdateFunc _inputUpdate;

    private Transform _rayOrigin;

    [SerializeField] private XRNode xrNodeL = XRNode.LeftHand;
    [SerializeField] private XRNode xrNodeR = XRNode.RightHand;
    private readonly List<InputDevice> _devicesL = new List<InputDevice>();
    private readonly List<InputDevice> _devicesR = new List<InputDevice>();
    private InputDevice _deviceL;
    private InputDevice _deviceR;
    [SerializeField] private Transform deviceLTransform;
    [SerializeField] private Transform deviceRTransform;
    
    // Start is called before the first frame update
    void Start()
    {

        if (GameManager.instance.InVR)
        {
            _inputUpdate = VRInputs;
            _cam = FindObjectOfType<XRRig>().GetComponentInChildren<Camera>();

            GetDevices();
        }
        else
        {
            _inputUpdate = MouseInputs;
            _cam = FindObjectOfType<PlayerMouseCameraControl>().GetComponentInChildren<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _inputUpdate();
        if(_shootRay && CardManager.instance.canPressButtons)
        {
            ButtonRay();
        }
    }

    void GetDevices()
    {
        InputDevices.GetDevicesAtXRNode(xrNodeL, _devicesL);
        InputDevices.GetDevicesAtXRNode(xrNodeR, _devicesR);
        _deviceL = _devicesL.FirstOrDefault();
        _deviceR = _devicesR.FirstOrDefault();
    }

    void MouseInputs()
    {
        if(UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
        {
            _shootRay = true;
            _rayOrigin = _cam.transform;
        }
    }
    
    void VRInputs()
    {
        if (!_deviceL.isValid || !_deviceR.isValid)
        {
            GetDevices();
        }

        _deviceL.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerL);
        if(triggerL) // FIGURE THIS OUT
        {
            _shootRay = true;
            _rayOrigin = deviceLTransform;
        }
        _deviceR.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerR);
        if(triggerR) // FIGURE THIS OUT
        {
            _shootRay = true;
            _rayOrigin = deviceRTransform;
        }
    }

    void ButtonRay()
    {
        Ray buttonRay = new Ray(_rayOrigin.transform.position, _rayOrigin.transform.forward); // Update this for different hands
        RaycastHit hit;
        if(Physics.Raycast(buttonRay, out hit, 10f))
        {
            Debug.DrawRay(buttonRay.origin, buttonRay.direction, Color.magenta);
            Debug.Log(hit.collider.gameObject);
            if(hit.collider.tag == "Button")
            {
                hit.collider.gameObject.GetComponent<AlienButton>().Pressed();
            }
        }

        _shootRay = false;
    }
}
