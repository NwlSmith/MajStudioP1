using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WinGameManager : MonoBehaviour
{
    public Volume Volume;

    public int timer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer++;

        if(timer < 400)
        {
            Volume.weight = timer / 400f;
        }
    }
}
