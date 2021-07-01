using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrToAudioClipDictionary : MonoBehaviour
{
    
    [Serializable]
    public struct StrToAc
    {
        [TextArea(5,100)]
        [SerializeField] public string str;
        [SerializeField] public AudioClip ac;
    }
    [SerializeField] private StrToAc[] monologuesToACs;

    private Dictionary<string, AudioClip> _dictionary = new Dictionary<string, AudioClip>();

    [SerializeField] private AudioClip defaultClip;

    void Awake()
    {

        foreach (StrToAc pair in monologuesToACs)
        {
            _dictionary.Add(pair.str, pair.ac);
        }
    }

    public AudioClip GetAc(string key)
    {
        Debug.Log($"Called dictionary for {key}");
        if (_dictionary.ContainsKey(key))
        {
            Debug.Log("found and returned clip");
            return _dictionary[key];
        }
        Debug.Log("Didn't find clip");
        return defaultClip;
    }
}
