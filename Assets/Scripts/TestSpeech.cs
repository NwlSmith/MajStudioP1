using UnityEngine;
using UnityLibrary;

public class TestSpeech : MonoBehaviour
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    public string sayAtStart = "Welcome!";

    // Start is called before the first frame update
    void Start()
    {
        // TEST speech
        TTSManager.instance.Say(sayAtStart, TTSCallback);

    }

    // Update is called once per frame
    void Update()
    {
        // test pressing any keys to say that character
        if (Input.anyKeyDown)
        {
            TTSManager.instance.Say(Input.inputString);
        }
    }

    void TTSCallback(string message, AudioClip audio) {
        AudioSource source = GetComponent<AudioSource>();
        if(source == null) {
            source = gameObject.AddComponent<AudioSource>();
        }

        source.clip = audio;
        source.Play();
    }
#endif
}
