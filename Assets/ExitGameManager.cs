using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameManager : MonoBehaviour
{
    public GameObject[] gObjs;
    public SpriteRenderer[] srs;

    bool endingGame;
    int timer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
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

        if(UnityEngine.InputSystem.Keyboard.current.escapeKey.wasReleasedThisFrame)
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
    }

    void FixedUpdate()
    {
        if(endingGame)
        {
            timer++;
        }

        srs[0].color = new Color(1, 1, 1, (Mathf.Clamp(timer, 0f, 50f) / 50f));

        srs[1].color = new Color(1, 1, 1, ((Mathf.Clamp(timer, 50f, 100f) - 50f) / 50f));

        srs[2].color = new Color(1, 1, 1, ((Mathf.Clamp(timer, 100f, 150f) - 100f) / 50f));

        if(timer > 175)
        {
            Application.Quit();
        }

    }
}
