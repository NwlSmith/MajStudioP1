﻿using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

/*
 * Date created: 10/25/2020
 * Creator: Nate Smith
 * 
 * Description: GameManager.
 * Handles most misc tasks, like Pausing, tying different elements together, and handling global controls.
 */
public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    [SerializeField] private bool _inVR = false;

    public bool InVR => _inVR;

    public Image fadeImg;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        // DETERMINE IF IN VR
        if (_inVR)
        {
            // remove Mouse and Keyboard controller
            FindObjectOfType<PlayerMouseCameraControl>().gameObject.SetActive(false);
        }
        else
        {
            // remove VR stuff
            FindObjectOfType<XRRig>().gameObject.SetActive(false);
            FindObjectOfType<VRChairRotator>().enabled = false;
        }

        TextMesh[] textMeshes = FindObjectsOfType<TextMesh>();
        foreach (TextMesh textMesh in textMeshes)
        {
            textMesh.GetComponent<MeshRenderer>().material.SetInt("unity_GUIZTestMode", (int)UnityEngine.Rendering.CompareFunction.LessEqual);
        }
        
        TextMeshPro[] textMeshPros = FindObjectsOfType<TextMeshPro>();
        foreach (TextMeshPro textMeshPro in textMeshPros)
        {
            textMeshPro.GetComponent<MeshRenderer>().material.SetInt("unity_GUIZTestMode", (int)UnityEngine.Rendering.CompareFunction.LessEqual);
        }
    }

    private void Start()
    {
        StartCoroutine(StartEnum());
    }

    private IEnumerator StartEnum()
    {
        fadeImg.color = new Color(0f, 0f, 0f, 1f);
        float duration = 3f;
        float elapsedTime = 0f;
        Color initColor = new Color(0f, 0f, 0f, 1f);
        Color finalColor = new Color(0f, 0f, 0f, 0f);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            fadeImg.color = Color.Lerp(initColor, finalColor, elapsedTime / duration);

            yield return null;
        }
        fadeImg.color = finalColor;

        DeckManager.instance.NextCard();
    }

    public void GameOverLose()
    {
        // fade to black, have a TTS thing say oh god what have you done or something
        StartCoroutine(GameOverLoseEnum());
    }

    private IEnumerator GameOverLoseEnum()
    {
        float duration = 2f;
        float elapsedTime = 0f;
        Color initColor = new Color(0f, 0f, 0f, 0f);
        Color finalColor = new Color(0f, 0f, 0f, 1f);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            fadeImg.color = Color.Lerp(initColor, finalColor, elapsedTime / duration);

            yield return null;
        }
        fadeImg.color = Color.Lerp(initColor, finalColor, elapsedTime / duration);

        SceneManager.LoadScene(3);
    }

    public void GameOverWin()
    {
        // fade to white, have a TTS thing say you reached enlightenment
        StartCoroutine(GameOverWinEnum());
    }

    private IEnumerator GameOverWinEnum()
    {
        float duration = 2f;
        float elapsedTime = 0f;
        Color initColor = new Color(1f, 1f, 1f, 0f);
        Color finalColor = new Color(1f, 1f, 1f, 1f);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            fadeImg.color = Color.Lerp(initColor, finalColor, elapsedTime / duration);

            yield return null;
        }
        fadeImg.color = Color.Lerp(initColor, finalColor, elapsedTime / duration);

        SceneManager.LoadScene(4);
    }

    public void DestroyAllManagers()
    {
        if (TTSManager.instance)
        {
            TTSManager.instance.StopSpeaking();
            Destroy(TTSManager.instance.gameObject);
        }

        if (DeckManager.instance)
        {
            DeckManager.instance.StopAllCoroutines();
            Destroy(DeckManager.instance.gameObject);
        }

        if (StatManager.instance)
        {
            StatManager.instance.StopAllCoroutines();
            Destroy(StatManager.instance.gameObject);
        }

        if (CardManager.instance)
        {
            CardManager.instance.StopAllCoroutines();
            Destroy(CardManager.instance.gameObject);
        }

        if (MusicManager.instance)
        {
            MusicManager.instance.StopMusic();
            Destroy(MusicManager.instance.gameObject);
        }

        if (RoommateManager.instance)
        {
            RoommateManager.instance.StopAllCoroutines();
            Destroy(RoommateManager.instance.gameObject);
        }
        
        Destroy(gameObject);
    }
}
