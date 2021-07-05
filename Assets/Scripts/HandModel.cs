using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HandModel : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private Transform mover;
    [SerializeField] private float smoothing;
    [SerializeField] private AudioClip impactSoundClip;
    private AudioSource _impactSoundAS;

    private void Awake()
    {
        _impactSoundAS = GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        mover.position = target.position;//smooth
        //mover.rotation = target.rotation;
    }
    private void Impact()
    {
        _impactSoundAS.pitch = Random.Range(.9f, 1.1f);
        _impactSoundAS.PlayOneShot(impactSoundClip);
    }
}
