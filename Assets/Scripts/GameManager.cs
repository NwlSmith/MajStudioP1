using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

    public Image fadeImg;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
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
        fadeImg.color = Color.Lerp(initColor, finalColor, elapsedTime / duration);

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

        SceneManager.LoadScene(2);
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

        SceneManager.LoadScene(3);
    }
}
