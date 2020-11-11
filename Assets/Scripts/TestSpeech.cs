using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpeech : MonoBehaviour
{
    public string sayAtStart = "Welcome!";

    // Start is called before the first frame update
    void Start()
    {
        // TEST speech

    }

    // Update is called once per frame
    void Update()
    {
        // test pressing any keys to say that character
        if (Input.anyKeyDown)
        {
            UnityWebGLSpeechSynthesis.SpeechSynthesisUtterance utterence = new UnityWebGLSpeechSynthesis.SpeechSynthesisUtterance();
            UnityWebGLSpeechSynthesis.WebGLSpeechSynthesisPlugin.GetInstance().Speak(utterence);
        }
    }
}
