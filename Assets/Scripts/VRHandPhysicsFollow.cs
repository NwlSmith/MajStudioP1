using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRHandPhysicsFollow : MonoBehaviour
{
    [SerializeField] private Transform hand = null;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log($"{name} starting");
    }

    private void FixedUpdate()
    {
        rb.MovePosition(hand.position);
    }
}
