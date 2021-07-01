// https://github.com/tgraupmann/UnityWebGLSpeechSynthesis

using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Collections;

public class TTSManager : MonoBehaviour
{
    // singleton isntance
    public static TTSManager instance = null;

    private AudioSource _audioSource;

    private StrToAudioClipDictionary _dictionary;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this) Destroy(gameObject);

        _audioSource = GetComponent<AudioSource>();
        _dictionary = GetComponent<StrToAudioClipDictionary>();
    }

    private float Say(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
        return clip.length;
    }
    
    public float Say(string str) => Say(_dictionary.GetAc(str));

    public void StopSpeaking() => _audioSource.Stop();
}