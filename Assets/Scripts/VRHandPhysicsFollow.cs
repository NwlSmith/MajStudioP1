using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class VRHandPhysicsFollow : MonoBehaviour
{
    [SerializeField] private Transform hand = null;
    private Rigidbody _rb;
    private float _smoothing = .2f;

    private HingeJoint[] _joints;

    [SerializeField] private bool _left = false;
    
    [SerializeField] private XRNode xrNodeL = XRNode.LeftHand;
    [SerializeField] private XRNode xrNodeR = XRNode.RightHand;
    private readonly List<InputDevice> _devices = new List<InputDevice>();
    private InputDevice _device;
    private bool pressed = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Debug.Log($"{name} starting");

        _joints = GetComponentsInChildren<HingeJoint>();
        
        GetDevices();
    }
    
    void GetDevices()
    {
        if (_left)
            InputDevices.GetDevicesAtXRNode(xrNodeL, _devices);
        else
            InputDevices.GetDevicesAtXRNode(xrNodeR, _devices);
        _device = _devices.FirstOrDefault();
    }

    private void Update()
    {
        // Detect input
        _device.TryGetFeatureValue(CommonUsages.gripButton, out bool trigger);
        if(trigger && !pressed)
        {
            pressed = true;
            SetJoints(true);
            Debug.Log($"Turning {name} on");
        }
        else if (!trigger && pressed)
        {
            pressed = false;
            SetJoints(false);
            Debug.Log($"Turning {name} off");
        }
    }

    private void SetJoints(bool turnOn)
    {
        foreach (HingeJoint joint in _joints)
        {
            joint.useMotor = turnOn;
        }
    }

    private void FixedUpdate()
    {
        Vector3 newPos = Vector3.Lerp(_rb.position, hand.position, _smoothing);
        _rb.MovePosition(newPos);
        Quaternion newRot = Quaternion.Slerp(_rb.rotation, hand.rotation, _smoothing);
        _rb.MoveRotation(newRot);
    }
}
