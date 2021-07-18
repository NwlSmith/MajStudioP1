
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Collections;
using Crosstales.RTVoice;

public class TTSManager : MonoBehaviour
{
    // singleton instance
    public static TTSManager instance = null;

    private AudioSource _audioSource;

    private StrToAudioClipDictionary _dictionary;

    [SerializeField][Tooltip("Divides the length of the string by this number. Smaller means longer speech.")] private float _speechLengthScaler = 10f;

    [SerializeField] private string _voiceName = ""; 

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

    public float Say(string str, int voiceNum = 0)
    {
        Speaker.Instance.Speak(str, _audioSource, Speaker.Instance.VoiceForName(_voiceName));
        return EstimatedSpeechLength(str);
        //Say(_dictionary.GetAc(str));
    }

    private float EstimatedSpeechLength(string str)
    {
        return str.Length / _speechLengthScaler;
    }

    public void StopSpeaking() => _audioSource.Stop();
}