﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienButton : MonoBehaviour
{
    public bool pressed;
    public bool wait;
    public bool reset;

    public bool button1 = false;

    Vector3 ogPos;

    public float pressDis;

    int timer;
    int timerLimit = 30;

    AudioSource aS;
    // Start is called before the first frame update
    void Start()
    {
        ogPos = transform.position;
        aS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(pressed)
        {
            transform.position = Vector3.Lerp(transform.position, ogPos + (transform.right * pressDis), 0.5f);
            if(Vector3.Distance(transform.position, ogPos + (transform.right * pressDis)) < 0.05f)
            {
                pressed = false;
                wait = true;
            }
        }

        if(wait)
        {
            timer++;
            if(timer > timerLimit)
            {
                timer = 0;
                wait = false;
                reset = true;
            }
        }

        if(reset)
        {
            transform.position = Vector3.Lerp(transform.position, ogPos, 0.2f);
            if (Vector3.Distance(transform.position, ogPos) < 0.05f)
            {
                reset = false;
            }
        }
    }

    public void Pressed()
    {
        pressed = true;

        aS.pitch = Random.Range(0.9f, 1.1f);
        aS.Play();

        if (button1)
            CardManager.instance.Chose1();
        else
            CardManager.instance.Chose2();
    }
}
