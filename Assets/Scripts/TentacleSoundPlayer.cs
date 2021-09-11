using System.Collections.Generic;
using UnityEngine;

public class TentacleSoundPlayer : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _clips = new List<AudioClip>();
    private List<AudioSource> _audioSources = new List<AudioSource>();
    private int curAS = 0;
    private int numAS = 1;

    private void Awake()
    {
        for (int i = 0; i < numAS; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
            audioSource.maxDistance = 10f;
            audioSource.volume = 0.15f;
            _audioSources.Add(audioSource);
        }
    }

    public void PlaySound()
    {
        if (!_audioSources[curAS].isPlaying)
        {
            _audioSources[curAS].pitch = UnityEngine.Random.Range(.9f, 1.1f);
        
            _audioSources[curAS].PlayOneShot(_clips[UnityEngine.Random.Range(0, _clips.Count)]);
        }

        curAS = (curAS + 1) % numAS;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlaySound();
    }
}
