using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHandPhysicsFollow : MonoBehaviour
{
    [SerializeField] private Transform hand = null;
    private Rigidbody _rb;
    private float _smoothing = .2f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Debug.Log($"{name} starting");
    }

    private void FixedUpdate()
    {
        Vector3 newPos = Vector3.Lerp(_rb.position, hand.position, _smoothing);
        _rb.MovePosition(newPos);
        Quaternion newRot = Quaternion.Slerp(_rb.rotation, hand.rotation, _smoothing);
        _rb.MoveRotation(newRot);
    }
}
