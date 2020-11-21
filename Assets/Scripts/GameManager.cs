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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GameOverLose()
    {
        // fade to black, have a TTS thing say oh god what have you done or something
    }

    public void GameOverWin()
    {
        // fade to white, have a TTS thing say you reached enlightenment
    }
}
