using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitGameManager : MonoBehaviour
{
    public GameObject[] gObjs;
    public Image[] images;

    bool endingGame;
    float timer;

    void Update()
    {
        if(UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame || MiscInput.instance.menuButtonClickedThisFrame)
        {
            StartEndGame();
        }

        if(UnityEngine.InputSystem.Keyboard.current.escapeKey.wasReleasedThisFrame || MiscInput.instance.menuButtonReleasedThisFrame)
        {
            EndEndGame();
        }
    }

    public void StartEndGame()
    {
        endingGame = true;

        if(gObjs[0].activeSelf == false)
        {
            for (int i = 0; i < gObjs.Length; i++)
            {
                gObjs[i].SetActive(true);
            }
        }
    }

    public void EndEndGame()
    {
        endingGame = false;
        timer = 0;

        if (gObjs[0].activeSelf == true)
        {
            for (int i = 0; i < gObjs.Length; i++)
            {
                gObjs[i].SetActive(false);
            }
        }
    }

    void FixedUpdate()
    {
        if (endingGame)
        {
            timer ++;//= Time.fixedDeltaTime;
        }
//        Debug.Log(images[0].color);
        if (images[0])
            images[0].color = new Color(1, 1, 1, (Mathf.Clamp(timer, 0f, 50f) / 50f));

        if (images[1])
            images[1].color = new Color(1, 1, 1, ((Mathf.Clamp(timer, 50f, 100f) - 50f) / 50f));

        if (images[2])
            images[2].color = new Color(1, 1, 1, ((Mathf.Clamp(timer, 100f, 150f) - 100f) / 50f));

        if (timer > 175)
        {
            Pressed();
        }
    }

    public void Pressed()
    {
        Application.Quit();
    }
}
