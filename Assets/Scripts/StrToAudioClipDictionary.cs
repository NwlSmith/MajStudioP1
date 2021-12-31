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
    [SerializeField] private List<StrToAc> strToACs = new List<StrToAc>();

    [SerializeField] private Dictionary<string, AudioClip> _dictionary = new Dictionary<string, AudioClip>();

    [SerializeField] private AudioClip defaultClip = null;

    void Awake()
    {

        foreach (StrToAc pair in strToACs)
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

    public void AddEntry(string key, AudioClip value)
    {
        if (_dictionary.ContainsKey(key))
        {
            Debug.Log($"Dictionary already contains clip {key}");
            return;
        }

        StrToAc strToAc = new StrToAc();
        strToAc.str = key;
        strToAc.ac = value;
        strToACs.Add(strToAc);
        _dictionary.Add(key, value);
    }
}
