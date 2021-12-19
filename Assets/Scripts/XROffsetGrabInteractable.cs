using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XROffsetGrabInteractable : XRGrabInteractable
{

    private Vector3 m_InitialAttachLocalPos; 
    private Quaternion m_InitialAttachLocalRot;
    
    private void Start()
    {
        InitialSetupAttachTransform();
    }

    private void InitialSetupAttachTransform()
    {
        if (!attachTransform)
        {
            GameObject grab = new GameObject("Grab Pivot");
            grab.transform.SetParent(transform, false);
            attachTransform = grab.transform;
        }

        m_InitialAttachLocalPos = attachTransform.localPosition;
        m_InitialAttachLocalRot = attachTransform.localRotation;
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        if (!attachTransform)
        {
            InitialSetupAttachTransform();
        }
        if (interactor is XRDirectInteractor)
        {
            Transform interactorTransform = interactor.transform;
            attachTransform.position = interactorTransform.position;
            attachTransform.rotation = interactorTransform.rotation;
        }
        else if (interactor is XRSocketInteractor)
        {
            attachTransform.localPosition = Vector3.zero;
            attachTransform.localRotation = Quaternion.identity;
        }
        else
        {
            attachTransform.position = m_InitialAttachLocalPos;
            attachTransform.rotation = m_InitialAttachLocalRot;
        }
        base.OnSelectEntered(interactor);
    }
}