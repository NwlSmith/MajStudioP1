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
    [SerializeField] private AudioClip[] characterSounds = null;

    [SerializeField] private List<GameObject> characterModels = null;
    [SerializeField] private GameObject scientistModel = null;
    [SerializeField] private GameObject undercoverModel = null;
    [SerializeField] private GameObject assimilationModel = null;
    [SerializeField] private GameObject corporateModel = null;
    [SerializeField] private GameObject clownModel = null;
    [SerializeField] private GameObject vacationModel = null;
    [SerializeField] private GameObject artistModel = null;
    [SerializeField] private GameObject lonelyModel = null;

    private GameObject curModel = null;

    private bool stopAnim = false;

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
                curModel = scientistModel;
                // set sounds
                audioSource.clip = characterSounds[0]; // NEEDS VOICE
                break;
            case AlienEnum.Undercover:
                // set model
                curModel = undercoverModel;
                // set sounds
                audioSource.clip = characterSounds[3];
                break;
            case AlienEnum.Assimilation:
                // set model
                curModel = assimilationModel;
                // set sounds
                audioSource.clip = characterSounds[4];
                break;
            case AlienEnum.Corporate:
                // set model
                curModel = corporateModel;
                // set sounds
                audioSource.clip = characterSounds[0]; // NEEDS VOICE
                break;
            case AlienEnum.Clown:
                // set model
                curModel = clownModel;
                // set sounds
                audioSource.clip = characterSounds[2];
                break;
            case AlienEnum.Vacation:
                // set model
                curModel = vacationModel;
                // set sounds
                audioSource.clip = characterSounds[1];
                break;
            case AlienEnum.Artist:
                // set model
                curModel = artistModel;
                // set sounds
                audioSource.clip = characterSounds[0]; // NEEDS VOICE
                break;
            case AlienEnum.Lonely:
                // set model
                curModel = lonelyModel;
                // set sounds
                audioSource.clip = characterSounds[0];
                break;
        }

        curModel.SetActive(true);
    }

    public void SpeakVisuals()
    {
        PlaySpeakingSound();
        //animator.SetTrigger("Talk"); // MAYBE REPLACE WITH SOMETHING ELSE LOL
    }

    private void PlaySpeakingSound()
    {
        audioSource.clip = characterSounds[Random.Range(0, characterSounds.Length)];
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }

    /*
     * Starts the audio and visual elements of the card.
     */
    public void Activate()
    {
        StartCoroutine(ActivateEnum());
    }

    private IEnumerator ActivateEnum()
    {
        // lerp to full size
        StartCoroutine(ShowVisuals());
        // speak a second later

        yield return new WaitForSeconds(1f);

        SpeakVisuals();

        stopAnim = false;

        StartCoroutine(IdleAnim());
    }

    public IEnumerator ShowVisuals()
    {
        curModel.SetActive(true);
        float duration = .3f;
        float elapsedTime = 0f;
        Vector3 initScale = new Vector3(curModel.transform.localScale.x, 0f, curModel.transform.localScale.z);
        Vector3 initRot = curModel.transform.rotation.eulerAngles;
        Vector3 targetScale = curModel.transform.localScale;
        Vector3 targetRot = curModel.transform.rotation.eulerAngles + new Vector3(0f, 3600f, 0f);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            curModel.transform.localScale = Vector3.Slerp(initScale, targetScale, elapsedTime / duration);
            curModel.transform.eulerAngles = Vector3.Slerp(initRot, targetRot, elapsedTime / duration);

            yield return null;
        }
        curModel.transform.localScale = targetScale;
        curModel.transform.eulerAngles = targetRot;
    }

    private IEnumerator IdleAnim()
    {
        Vector3 realInitScale = curModel.transform.localScale;
        Vector3 realInitRot = curModel.transform.eulerAngles;
        while (!stopAnim)
        {
            float duration = 1f;
            float elapsedTime = 0f;
            Vector3 initScale = curModel.transform.localScale;
            Vector3 initRot = curModel.transform.rotation.eulerAngles;
            Vector3 targetScale = curModel.transform.localScale + new Vector3(0f, .1f, 0f);
            Vector3 targetRot = curModel.transform.rotation.eulerAngles + new Vector3(0f, 5f, 0f);
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                curModel.transform.localScale = Vector3.Slerp(initScale, targetScale, elapsedTime / duration);
                curModel.transform.eulerAngles = Vector3.Slerp(initRot, targetRot, elapsedTime / duration);

                yield return null;
            }

            curModel.transform.localScale = targetScale;
            curModel.transform.eulerAngles = targetRot;

            elapsedTime = 0f;
            initScale = curModel.transform.localScale;
            initRot = curModel.transform.rotation.eulerAngles;
            targetScale = realInitScale;
            targetRot = realInitRot;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                curModel.transform.localScale = Vector3.Slerp(initScale, targetScale, elapsedTime / duration);
                curModel.transform.eulerAngles = Vector3.Slerp(initRot, targetRot, elapsedTime / duration);

                yield return null;
            }

            curModel.transform.localScale = targetScale;
            curModel.transform.eulerAngles = targetRot;
        }

        curModel.transform.localScale = realInitScale;
        curModel.transform.eulerAngles = realInitRot;
    }

    /*
     * Stops the audio and visual elements of the card.
     */
    public void Deactivate()
    {
        stopAnim = true;
        StartCoroutine(DeactivateEnum());
    }

    private IEnumerator DeactivateEnum()
    {
        yield return new WaitForSeconds(.5f);
        StartCoroutine(HideVisuals());
    }

    public IEnumerator HideVisuals()
    {
        float duration = .3f;
        float elapsedTime = 0f;
        Vector3 initScale = curModel.transform.localScale;
        Vector3 initRot = curModel.transform.rotation.eulerAngles;
        Vector3 targetScale = new Vector3(curModel.transform.localScale.x, 0f, curModel.transform.localScale.z);
        Vector3 targetRot = curModel.transform.rotation.eulerAngles + new Vector3(0f, 3600f, 0f);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            curModel.transform.localScale = Vector3.Slerp(initScale, targetScale, elapsedTime / duration);
            curModel.transform.eulerAngles = Vector3.Slerp(initRot, targetRot, elapsedTime / duration);

            yield return null;
        }
        curModel.transform.localScale = targetScale;
        curModel.transform.eulerAngles = targetRot;
        yield return null;
        curModel.SetActive(false);
        curModel.transform.localScale = initScale;
    }
}
