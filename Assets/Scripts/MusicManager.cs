using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance = null;

    private AudioSource audioSource = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartMusic()
    {
        audioSource.Play();
    }

    public void StopMusic()
    {
        if (audioSource)
            audioSource.Stop();
    }
}
