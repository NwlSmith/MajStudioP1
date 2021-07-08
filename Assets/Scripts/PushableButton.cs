using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class PushableButton : XRBaseInteractable
{
    public bool pushable = true;
    
    private bool _pressed = false;

    private float _yMin = 0.0f;
    private float _yMax = 0.0f;

    private float _previousHandHeight = 0.0f;
    private XRBaseInteractor _hoverInteractor = null;
    private bool _previousPress = false;

    private AudioSource _audioSource;

    protected override void Awake()
    {
        base.Awake();
        onHoverEntered.AddListener(StartedPressing);
        onHoverExited.AddListener(StoppedPressing);
        _audioSource = GetComponent<AudioSource>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onHoverEntered.RemoveListener(StartedPressing);
        onHoverEntered.RemoveListener(StoppedPressing);
    }

    private void Start()
    {
        SetMinMax();
    }

    private void SetMinMax()
    {
        Collider collider = GetComponent<Collider>();
        _yMin = transform.localPosition.y - (collider.bounds.size.y * 0.5f);
        _yMax = transform.localPosition.y;
    }

    private void StartedPressing(XRBaseInteractor interactor)
    {
        Debug.Log("Started Pressing");
        _hoverInteractor = interactor;
        _previousHandHeight = GetLocalYPosition(interactor.transform.position);
    }

    private void StoppedPressing(XRBaseInteractor interactor)
    {
        Debug.Log("Stopped Pressing");
        _hoverInteractor = null;
        _previousHandHeight = 0.0f;

        _previousPress = false;
        SetYPosition(_yMax);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (_hoverInteractor)
        {
            if (!CheckCanPressButtons())
            {
                StoppedPressing(_hoverInteractor);
                return;
            }
            
            float newHandHeight = GetLocalYPosition(_hoverInteractor.transform.position);
            float handDifference = _previousHandHeight - newHandHeight;
            _previousHandHeight = newHandHeight;

            float newPosition = transform.localPosition.y - handDifference;
            
            SetYPosition(newPosition);
            
            CheckPress();
        }
    }
    
    protected virtual bool CheckCanPressButtons()
    {
        return true;
    }

    private float GetLocalYPosition(Vector3 position)
    {
        Vector3 localPosition = transform.root.InverseTransformPoint(position);
        return localPosition.y;
    }

    private void SetYPosition(float position)
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.y = Mathf.Clamp(position, _yMin, _yMax);
        transform.localPosition = newPosition;
    }

    private void CheckPress()
    {
        bool inPosition = InPosition();
        
        if(!_pressed && inPosition && inPosition != _previousPress)
            Pressed();

        _previousPress = inPosition;
    }

    private bool InPosition()
    {
        float inRange = Mathf.Clamp(transform.localPosition.y, _yMin, _yMin + 0.01f);
        
        return transform.localPosition.y == inRange;
    }

    public virtual void Pressed()
    {
        _audioSource.Play();
        StartCoroutine(ReturnButtonToNormalEnum());
    }

    private IEnumerator ReturnButtonToNormalEnum()
    {
        _pressed = true;
        float elapsedTime = 0;
        float duration = .5f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            SetYPosition(Mathf.Lerp(_yMin, _yMax, elapsedTime / duration));
            yield return null;
        }
        SetYPosition(_yMax);
        _pressed = false;
    }
}
