using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class IntroManager : MonoBehaviour
{
    public Image fadeImg;

    private PushableButton[] _buttons;

    void Start()
    {
        _buttons = FindObjectsOfType<PushableButton>();
        StartCoroutine(InitEnum());
    }

    private IEnumerator InitEnum()
    {
        foreach (var button in _buttons)
        {
            button.SetButtonEnabled(false);
        }
        
        yield return new WaitForSeconds(2f);
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
        fadeImg.color = Color.Lerp(initColor, finalColor, elapsedTime / duration);
        
        foreach (var button in _buttons)
        {
            button.SetButtonEnabled(true);
        }
    }

    public void PressStart()
    {
        StartCoroutine(StartGameEnum());
        GetComponent<AudioSource>().Play();
    }

    private IEnumerator StartGameEnum()
    {
        foreach (var button in _buttons)
        {
            button.SetButtonEnabled(false);
        }
        
        float duration = 3f;
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
        
        AsyncOperation loading = SceneManager.LoadSceneAsync("Finn_Cockpit");

        DontDestroyOnLoad(this);

        while (!loading.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(.1f);
        
        Destroy(gameObject);
    }

    public void NextScene()
    {
        Debug.Log("Transitioning to Finn_Cockpit");
        SceneManager.LoadScene("Finn_Cockpit");
    }
}