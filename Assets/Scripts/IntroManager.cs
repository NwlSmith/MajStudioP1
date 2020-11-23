using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public Image fadeImg;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitEnum());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator InitEnum()
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
        fadeImg.color = Color.Lerp(initColor, finalColor, elapsedTime / duration);
    }

    public void PressStart()
    {
        StartCoroutine(StartGameEnum());
        GetComponent<AudioSource>().Play();
    }

    private IEnumerator StartGameEnum()
    {
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

        SceneManager.LoadScene(1);
    }
}
