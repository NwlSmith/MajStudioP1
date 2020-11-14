using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoommateManager : MonoBehaviour
{

    public static RoommateManager instance = null;

    [SerializeField]
    private List<string> monologueQueue = null;

    [SerializeField]
    private AudioClip[] speakSounds;

    private AudioSource audioSource;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    // UNIMPLEMENTED
    // Make the roommate  the given response to the Roommates 
    public void AddResponse(string response)
    {
        // IMPLEMENT
    }

    public void RoommateResponse(Card card, bool choice1)
    {
        // IMPLEMENT
        string words = "";
        int rand = Random.Range(1, 3);
        // either say the response, the monologue, or nothing.
        switch (rand)
        {
            // response
            case 1:
                // if chose 1
                if (choice1 && !card.d1RoommateResponseSpoken)
                {
                    words = card.d1RoommateResponse;
                    card.d1RoommateResponseSpoken = true;
                }
                // if chose 2
                else if (!choice1 && !card.d2RoommateResponseSpoken)
                {
                    words = card.d2RoommateResponse;
                    card.d2RoommateResponseSpoken = true;
                }
                // if choice was said before
                else
                {
                    rand = Random.Range(1, 2);
                    switch (rand)
                    {
                        case 1:
                            words = Monologue();
                            break;
                    }
                }
                break;
            // monologue
            case 2:
                words = Monologue();
                break;
            // response
        }
        Speak(words);
    }

    private string Monologue()
    {
        string s = "";
        if (monologueQueue.Count > 0)
        {
            s = monologueQueue[0];
            monologueQueue.RemoveAt(0);
        }
        return s;
    }

    public void Speak(string response)
    {
        StartCoroutine(SpeakEnum(response));
    }

    private IEnumerator SpeakEnum(string response)
    {
        Debug.Log("reponse = " + response);
        // make sound
        audioSource.clip = speakSounds[Random.Range(0, speakSounds.Length - 1)];
        audioSource.pitch = Random.Range(.8f, 1.2f);
        if (!response.Equals(""))
        {
            audioSource.Play();
        }
        // delay
        yield return new WaitForSeconds(1f);
        // start translating, if there is a response
        if (!response.Equals(""))
        {
            UnityWebGLSpeechSynthesis.TTSManager.instance.Say(response, 1);
            // delay
            yield return new WaitForSeconds(10f);
        }
        else
        {
            // delay
            yield return new WaitForSeconds(Random.Range(6, 10));
        }
        // draw new card

        DeckManager.instance.NextCard();
    }
}
