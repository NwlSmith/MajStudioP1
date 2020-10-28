using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Date created: 10/27/2020
 * Creator: Nate Smith
 * 
 * Description: Handles the visuals of the cards.
 * 3D Model for Hologram
 * Simple animation + controller
 * Hologram shader
 * Character specific Sounds
 */
public class CardVisuals : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] characterSounds;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SpeakVisuals()
    {
        PlaySpeakingSound();
        //animator.SetTrigger("Talk"); // MAYBE REPLACE WITH SOMETHING ELSE LOL
        
    }

    private void PlaySpeakingSound()
    {
        audioSource.clip = characterSounds[Random.Range(0, characterSounds.Length)];
        audioSource.Play();
    }
}
