using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance = null;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        GetComponent<AudioSource>();
    }

    public void StartMusic()
    {
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}
