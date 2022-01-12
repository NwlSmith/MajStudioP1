using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class PushableButton : XRBaseInteractable
{
    
    public bool pushable = true;
    
    private bool _Pressed = false;

    private float _YMin = 0.0f;
    private float _YMax = 0.0f;

    private XRDirectInteractor _HoverInteractor = null;
    private bool _PreviousPress = false;

    private Coroutine _ReturnToNormalPosCO = null;
    private bool _ReturnToNormalPosCOActive = false;

    private float _InitCollisionLocalYPosition = 0f;

    private AudioSource _audioSource;

    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        SetMinMax();
        
        SetButtonEnabled(true);
    }

    private void SetMinMax()
    {
        Collider collider = GetComponent<Collider>();
        _YMin = transform.localPosition.y - (collider.bounds.size.y * 0.5f);
        _YMax = transform.localPosition.y;
    }

    protected override void OnHoverEntered(XRBaseInteractor interactor)
    {
        Debug.Log($"{interactor.name} started pressing button");
        if (pushable)
        {
            base.OnHoverEntered(interactor);
            StartedPressing(interactor);
        }
    }
    
    protected override void OnHoverExited(XRBaseInteractor interactor)
    {
        if (pushable)
        {
            base.OnHoverExited(interactor);
            StoppedPressing();
        }
    }

    private void StartedPressing(XRBaseInteractor interactor)
    {
        Debug.Log($"{interactor.name} started pressing {name}");
        if (interactor is XRDirectInteractor hand)
        {
            if (_HoverInteractor != hand)
            {
                _HoverInteractor = hand;
                _InitCollisionLocalYPosition =
                    transform.parent.InverseTransformPoint(_HoverInteractor.transform.position).y;
            }
            TryCancelReturnButtonToNormal();
        }
    }

    private void StoppedPressing()
    {
        //Debug.Log("Stopped Pressing");
        _HoverInteractor = null;

        _PreviousPress = false;
        _Pressed = false;
        TryReturnButtonToNormal();
        //SetYPosition(_YMax);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        if (updatePhase != XRInteractionUpdateOrder.UpdatePhase.Fixed) return;

        ProcessButtonPress();
    }

    private void ProcessButtonPress()
    {
        if (_HoverInteractor)
        {
            if (!CheckCanPressButtons())
            {
                StoppedPressing();
                return;
            }

            float collisionLocalYPosition = transform.parent.InverseTransformPoint(_HoverInteractor.transform.position).y;
            float difference = _InitCollisionLocalYPosition - collisionLocalYPosition;
            //Debug.Log($"_InitCollisionLocalYPosition = {_InitCollisionLocalYPosition}, collisionLocalYPosition = {collisionLocalYPosition}, dif = {difference}, ymax = {_YMax}, ymin = {_YMin}, setting position to {_YMax - difference}");
            SetYPosition(_YMax - difference);
            
            CheckPress();
        }
    }

    protected virtual bool CheckCanPressButtons()
    {
        return pushable;
    }

    private void SetYPosition(float position)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.y = Mathf.Clamp(position, _YMin, _YMax);
        transform.localPosition = newPosition;
    }

    private void CheckPress()
    {
        bool inPosition = InPosition();
        
        if(!_Pressed && inPosition && inPosition != _PreviousPress)
        {
            ButtonFullyPressed();
        }
        else
        {
            _Pressed = false;
        }

        _PreviousPress = inPosition;
    }

    private bool InPosition()
    {
        float inRange = Mathf.Clamp(transform.localPosition.y, _YMin, _YMin + 0.01f);
        
        return transform.localPosition.y == inRange;
    }

    public virtual void ButtonFullyPressed()
    {
        _Pressed = true;
        OnButtonPressed();
        _HoverInteractor.SendHapticImpulse(.25f, .05f);
        //StartCoroutine(ReturnButtonToNormalEnum());
    }

    protected virtual void OnButtonPressed()
    {
        _audioSource.Play();
        Debug.Log("Pressed Button");
    }

    private void TryReturnButtonToNormal()
    {
        if (_ReturnToNormalPosCOActive)
            return;

        _ReturnToNormalPosCO = StartCoroutine(ReturnButtonToNormalEnum());
    }
    
    private void TryCancelReturnButtonToNormal()
    {
        if (!_ReturnToNormalPosCOActive)
            return;

        StopCoroutine(_ReturnToNormalPosCO);
        _ReturnToNormalPosCOActive = false;
    }

    private IEnumerator ReturnButtonToNormalEnum()
    {
        _ReturnToNormalPosCOActive = true;
        
        _Pressed = true;
        float elapsedTime = 0;
        float duration = .25f;
        float initPos = transform.localPosition.y;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            SetYPosition(Mathf.Lerp(initPos, _YMax, elapsedTime / duration));
            yield return null;
        }
        SetYPosition(_YMax);
        _Pressed = false;
        
        _ReturnToNormalPosCOActive = false;
    }

    private IEnumerator SetToDeactivatedPositionEnum()
    {
        _Pressed = false;
        float elapsedTime = 0;
        float duration = .25f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            SetYPosition(Mathf.Lerp(transform.localPosition.y, _YMin, elapsedTime / duration));
            yield return null;
        }
        SetYPosition(_YMin);
    }

    public void SetButtonEnabled(bool newEnabled)
    {
        pushable = newEnabled;

        if (newEnabled)
        {
            TryReturnButtonToNormal();
        }
        else
        {
            StartCoroutine(SetToDeactivatedPositionEnum());
            _HoverInteractor = null;
        }
    }
}