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

    private void Start()
    {
        xrRig = GetComponent<XRRig>();
        initPos = xrRig.transform.position;

        if (altPos == null)
        {
            var altPosGO = new GameObject();
            altPosGO.name = "SitStandAltPos";
            altPosGO.transform.parent = xrRig.transform.parent;
            altPosGO.transform.position =
                new Vector3(xrRig.transform.position.x, xrRig.transform.position.y - 1, xrRig.transform.position.z);
            altPos = altPosGO.transform;
        }
    }
    
    

    private void Update()
    {
        currentCooldownTime -= Time.deltaTime;

        if (MiscInput.instance.thumbstickClicked && currentCooldownTime <= 0f)
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
