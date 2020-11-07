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
    // fields for each model 
    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] characterSounds;

    [SerializeField] private List<GameObject> characterModels;
    [SerializeField] private GameObject scientistModel;
    [SerializeField] private GameObject undercoverModel;
    [SerializeField] private GameObject assimilationModel;
    [SerializeField] private GameObject corporateModel;
    [SerializeField] private GameObject clownModel;
    [SerializeField] private GameObject vacationModel;
    [SerializeField] private GameObject artistModel;
    [SerializeField] private GameObject lonelyModel;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        characterModels = new List<GameObject>();
        characterModels.Insert(0, scientistModel);
        characterModels.Insert(0, undercoverModel);
        characterModels.Insert(0, assimilationModel);
        characterModels.Insert(0, corporateModel);
        characterModels.Insert(0, clownModel);
        characterModels.Insert(0, vacationModel);
        characterModels.Insert(0, artistModel);
        characterModels.Insert(0, lonelyModel);

        DisableModels();
    }

    private void DisableModels()
    {
        foreach (GameObject gameObject in characterModels.ToArray())
        {
            gameObject.SetActive(false);
        }
    }

    public void NewCard(AlienEnum alien)
    {
        DisableModels();
        switch (alien)
        {
            case AlienEnum.Scientist:
                // set model
                scientistModel.SetActive(true);
                // set sounds
                break;
            case AlienEnum.Undercover:
                // set model
                undercoverModel.SetActive(true);
                // set sounds
                break;
            case AlienEnum.Assimilation:
                // set model
                assimilationModel.SetActive(true);
                // set sounds
                break;
            case AlienEnum.Corporate:
                // set model
                corporateModel.SetActive(true);
                // set sounds
                break;
            case AlienEnum.Clown:
                // set model
                clownModel.SetActive(true);
                // set sounds
                break;
            case AlienEnum.Vacation:
                // set model
                vacationModel.SetActive(true);
                // set sounds
                break;
            case AlienEnum.Artist:
                // set model
                artistModel.SetActive(true);
                // set sounds
                break;
            case AlienEnum.Lonely:
                // set model
                lonelyModel.SetActive(true);
                // set sounds
                break;
        }
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
