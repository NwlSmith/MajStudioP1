using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SitStandToggle : MonoBehaviour
{
    [SerializeField] private Transform altPos;
    private Vector3 initPos;

    private XRRig xrRig = null;

    private bool inInitPos = true;

    private float cooldown = .2f;
    private float currentCooldownTime = 0f;
    
    private XRNode xrNodeR = XRNode.RightHand;
    private readonly List<InputDevice> _devicesR = new List<InputDevice>();
    private InputDevice _deviceR;

    private void Start()
    {
        xrRig = GetComponent<XRRig>();
        initPos = xrRig.transform.position;
        
        GetDevices();
    }
    
    void GetDevices()
    {
        InputDevices.GetDevicesAtXRNode(xrNodeR, _devicesR);
        _deviceR = _devicesR.FirstOrDefault();
    }

    private void Update()
    {
        currentCooldownTime -= Time.deltaTime;
        
        if (!_deviceR.isValid)
        {
            GetDevices();
        }

        _deviceR.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool clicked);

        if (clicked && currentCooldownTime <= 0f)
        {
            currentCooldownTime = cooldown;
            ToggleSitStand();
        }
    }

    public void ToggleSitStand()
    {
        xrRig.transform.position = inInitPos ? altPos.position : initPos;
        inInitPos = !inInitPos;
    }
}
