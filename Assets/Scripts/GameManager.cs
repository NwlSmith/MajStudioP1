using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            TTSManager.instance.Speak("Hello Terry the Spider, you are so cool");
        }
    }
}
