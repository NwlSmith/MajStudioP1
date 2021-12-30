using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class MiscInput : MonoBehaviour
{

    public static MiscInput instance = null;
    
    private XRNode xrNodeR = XRNode.RightHand;
    private readonly List<InputDevice> _devicesR = new List<InputDevice>();
    private InputDevice _deviceR;
    
    public bool thumbstickClicked { get; private set; }
    public bool primaryButtonClicked { get; private set; }
    

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        GetDevices();
    }
    
    void GetDevices()
    {
        InputDevices.GetDevicesAtXRNode(xrNodeR, _devicesR);
        _deviceR = _devicesR.FirstOrDefault();
    }

    void Update()
    {
        if (!_deviceR.isValid)
        {
            GetDevices();
        }

        _deviceR.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool thumbstick);
        thumbstickClicked = thumbstick;
        _deviceR.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButton);
        primaryButtonClicked = primaryButton;
    }
}
