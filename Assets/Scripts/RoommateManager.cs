using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoommateManager : MonoBehaviour
{

    public static RoommateManager instance = null;

    [SerializeField]
    [TextArea(4, 50)]
    private List<string> monologueQueue = null;

    [SerializeField]
    private AudioClip[] speakSounds = null;

    [SerializeField]
    private GameObject model = null;

    private bool stopAnim = false;

    public TMPro.TextMeshPro charDialogue;
    public TMPro.TextMeshPro oldDialogue1;
    public TMPro.TextMeshPro oldDialogue2;

    private AudioSource audioSource;

    private bool firstCardRead = false;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        charDialogue.text = "";
        oldDialogue1.text = "";
        oldDialogue2.text = "";
    }

    // UNIMPLEMENTED
    // Make the roommate  the given response to the Roommates 
    public void AddResponse(string response)
    {
        // IMPLEMENT
    }

    public void RoommateResponse(Card card, bool choice1)
    {
        bool isMonologue = false;
        if (!firstCardRead)
        {
            firstCardRead = true;
            isMonologue = true;
            Speak("yo dude, I have some weird shit I'm gonna tell ya later.", isMonologue);
            return;
        }
        string words = "";
        int rand = Random.Range(1, 4);
        if (rand == 4)
        {
            Debug.Log("You goofed");
            rand = 3;
        }
        // either say the response, the monologue, or nothing.
        switch (rand)
        {
            // response
            case 1:
                // if chose 1
                if (choice1 && !card.d1RoommateResponse.Equals("") && !card.d1RoommateResponseSpoken)
                {
                    words = card.d1RoommateResponse;
                    card.d1RoommateResponseSpoken = true;
                    charDialogue.color = new Color(1f, 0f, 1f, 1f);
                }
                // if chose 2
                else if (!choice1 && !card.d2RoommateResponse.Equals("") && !card.d2RoommateResponseSpoken)
                {
                    words = card.d2RoommateResponse;
                    card.d2RoommateResponseSpoken = true;
                    charDialogue.color = new Color(1f, 0f, 1f, 1f);
                }
                // if choice was said before
                else
                {
                    rand = Random.Range(1, 2);
                    switch (rand)
                    {
                        case 1:
                            isMonologue = true;
                            words = Monologue();
                            break;
                        case 2:
                            words = "";
                            break;

                    }
                }
                break;
            // monologue
            case 2:
                words = Monologue();
                isMonologue = true;
                break;
            // response
            case 3:
                words = "";
                break;
        }
        Speak(words, isMonologue);
    }

    private string monologueBack1;
    private string monologueBack2;
    private string Monologue()
    {
        string s = "";
        if (monologueQueue.Count > 0)
        {
            s = monologueQueue[0];
            if(monologueBack1 != "")
            {
                monologueBack2 = monologueBack1;                
            }
            monologueBack1 = monologueQueue[0];
            monologueQueue.RemoveAt(0);
        }
        charDialogue.color = Color.cyan;
        return s;
    }

    private void ChangeBacklog(string response)
    {
        oldDialogue2.text = oldDialogue1.text;
        oldDialogue1.text = response;
    }

    public void Speak(string response, bool isMonologue)
    {
        StartCoroutine(SpeakEnum(response, isMonologue));
    }

    private IEnumerator SpeakEnum(string response, bool isMonologue)
    {
        if (!response.Equals(""))
        {
            yield return new WaitForSeconds(Random.Range(2, 4));
            // make sound
            audioSource.clip = speakSounds[Random.Range(0, speakSounds.Length - 1)];
            audioSource.pitch = Random.Range(.8f, 1.2f);
            if (!response.Equals(""))
            {
                audioSource.Play();

                StartCoroutine(IdleAnim());
            }
            charDialogue.text = "Translating...";
            // delay
            yield return new WaitForSeconds(1f);
            // start translating, if there is a response
            charDialogue.text = response;
        }
        if (!response.Equals(""))
        {
            float delay = TTSManager.instance.Say(response, 2);
            // delay
            yield return new WaitForSeconds(delay);
        }
        else
        {
            // delay
            yield return new WaitForSeconds(Random.Range(2, 4));
        }
        charDialogue.text = "";
        if (isMonologue)
            ChangeBacklog(response);
        // draw new card
        stopAnim = true;

        if (monologueQueue.Count <= 0)
        {
            // player wins
            GameManager.instance.GameOverWin();
        }
        else
            DeckManager.instance.NextCard();
    }

    private IEnumerator IdleAnim()
    {
        stopAnim = false;
        Vector3 realInitScale = model.transform.localScale;
        Vector3 realInitRot = model.transform.eulerAngles;
        while (!stopAnim)
        {
            float duration = 1f;
            float elapsedTime = 0f;
            Vector3 initScale = model.transform.localScale;
            Vector3 initRot = model.transform.rotation.eulerAngles;
            Vector3 targetScale = model.transform.localScale + new Vector3(.003f, .003f, 0f);
            Vector3 targetRot = model.transform.rotation.eulerAngles + new Vector3(0f, 5f, 0f);
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                model.transform.localScale = Vector3.Slerp(initScale, targetScale, elapsedTime / duration);
                model.transform.eulerAngles = Vector3.Slerp(initRot, targetRot, elapsedTime / duration);

                yield return null;
            }

            model.transform.localScale = targetScale;
            model.transform.eulerAngles = targetRot;

            elapsedTime = 0f;
            initScale = model.transform.localScale;
            initRot = model.transform.rotation.eulerAngles;
            targetScale = realInitScale;
            targetRot = realInitRot;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;

                model.transform.localScale = Vector3.Slerp(initScale, targetScale, elapsedTime / duration);
                model.transform.eulerAngles = Vector3.Slerp(initRot, targetRot, elapsedTime / duration);

                yield return null;
            }

            model.transform.localScale = targetScale;
            model.transform.eulerAngles = targetRot;
        }

        model.transform.localScale = realInitScale;
        model.transform.eulerAngles = realInitRot;
    }
}
