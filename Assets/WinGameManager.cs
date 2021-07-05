using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;

public class WinGameManager : MonoBehaviour
{
    public Volume Volume;

    public int timer;

    [SerializeField] private bool inVR = true;
    [SerializeField] private Camera mainCam;
    
    void Start()
    {
        if (!inVR)
        {
            XRRig rig = FindObjectOfType<XRRig>();
            rig.gameObject.SetActive(false);
            
        }
        else
        {
            //FindObjectOfType<Canvas>().gameObject.SetActive(false);
            mainCam.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.rKey.wasPressedThisFrame)
        {
            ButtonPressed();
        }
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
    
    public void ButtonPressed()
    {
        SceneManager.LoadScene("Finn_Cockpit");
    }
}
