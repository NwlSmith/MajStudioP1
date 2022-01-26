using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class MiscInput : MonoBehaviour
{

    public static MiscInput instance = null;
    
    private XRNode xrNodeL = XRNode.RightHand;
    private XRNode xrNodeR = XRNode.RightHand;
    private readonly List<InputDevice> _devicesL = new List<InputDevice>();
    private readonly List<InputDevice> _devicesR = new List<InputDevice>();
    private InputDevice _deviceL;
    private InputDevice _deviceR;
    
    public bool thumbstickClicked { get; private set; }
    public bool primaryButtonClicked { get; private set; }
    public bool menuButtonClickedThisFrame { get; private set; }
    public bool menuButtonReleasedThisFrame { get; private set; }
    private bool _menuButtonClicked;
    

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        initTimeScale = Time.timeScale;
    }

    void Start()
    {
        GetDevices();
    }
    
    void GetDevices()
    {
        InputDevices.GetDevicesAtXRNode(xrNodeR, _devicesR);
        _deviceR = _devicesR.FirstOrDefault();
        InputDevices.GetDevicesAtXRNode(xrNodeL, _devicesL);
        _deviceL = _devicesL.FirstOrDefault();
    }

    void Update()
    {
        if (!_deviceR.isValid)
        {
            GetDevices();
        }

        _deviceR.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool thumbstick);
        thumbstickClicked = thumbstick;
        _deviceR.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButton);
        primaryButtonClicked = primaryButton;
        _deviceL.TryGetFeatureValue(CommonUsages.menuButton, out bool menuButton);

        if (menuButton && !menuButtonClickedThisFrame && !_menuButtonClicked)
        {
            menuButtonClickedThisFrame = true;
            _menuButtonClicked = true;
        }
        else if (menuButton && menuButtonClickedThisFrame && _menuButtonClicked)
        {
            menuButtonClickedThisFrame = false;
        }
        else if (!menuButton && !menuButtonReleasedThisFrame && _menuButtonClicked)
        {
            menuButtonReleasedThisFrame = true;
            _menuButtonClicked = false;
        }
        else if (!menuButton && menuButtonReleasedThisFrame && !_menuButtonClicked)
        {
            menuButtonReleasedThisFrame = false;
        }

        // Might work better for PC tethered VR
        /*if (!paused && IsPaused())
        {
            paused = true;
            Debug.Log("Detected Paused!!!");
            PauseGame(true);
            //OnApplicationFocus(false);
        }
        else if (paused && !IsPaused())
        {
            paused = false;
            Debug.Log("Detected Unpaused!!!");
            PauseGame(false);
            //OnApplicationFocus(true);
        }*/
    }

    private bool IsPaused()
    {
        return !OVRManager.hasInputFocus || !OVRManager.hasVrFocus;
    }

    private bool paused = false;

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Debug.Log("On Application has focus!");
            PauseGame(false);
        }
        else
        {
            Debug.Log("On Application has LOST focus!");
            PauseGame(true);
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Debug.Log("On Application Paused!");
            PauseGame(true);
        }
        else
        {
            Debug.Log("On Application UNPaused!");
            PauseGame(false);
        }
    }

    private float initTimeScale = 0;
    private float pausedTimeScale = .001f;

    private AudioSource[] _audioSources;
    
    private void PauseGame(bool pausing)
    {
        if (pausing)
        {
            Debug.Log("Pausing... Timescale = " + Time.timeScale);
            _audioSources = FindObjectsOfType<AudioSource>();
            foreach (var audioSource in _audioSources)
            {
                audioSource.Pause();
            }
            Time.timeScale = pausedTimeScale;
        }
        else
        {
            Debug.Log("Unpausing...");
            if (_audioSources != null)
            {
                foreach (var audioSource in _audioSources)
                {
                    if (audioSource)
                        audioSource.UnPause();
                }
            }
            Time.timeScale = initTimeScale;
        }
    }
}