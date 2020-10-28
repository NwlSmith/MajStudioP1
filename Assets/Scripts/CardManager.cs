using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * Date created: 10/27/2020
 * Creator: Nate Smith
 * 
 * Description: Base card class.
 * Will be implemented via prefabs for each character's. Takes in JSON(?) data to create the text-to-speech.
 * Each Card has:
 *   Character prefab
 *     3D Model for Hologram
 *     Simple animation + controller
 *     Hologram shader
 *     Character specific Sounds
 *   Text : Name + Dialogue for Decision
 *     text-to-speech
 *   Decision
 *     Flavor text for both buttons
 *     Stat changes
 *     Roommate response text, only plays when look back at them
 *     Boolean said already (oof)
 *     Reference to friend
 *     Cards that get shuffled into deck

 */
public class CardManager : MonoBehaviour
{

    //[SerializeField] private CardVisuals cardVisuals { get; set; }
    [SerializeField] private CardVisuals cardVisuals;

    //[SerializeField] private Card cardInfo { get; set; }
    [SerializeField] private Card cardInfo;

    public Text tempMainText;
    public Text tempD1Text;
    public Text tempD2Text;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            tempMainText.text = cardInfo.infoText;
            tempD1Text.text = cardInfo.decision1Text;
            tempD2Text.text = cardInfo.decision2Text;
            Activate();
        }
    }

    /*
     * Starts the audio and visual elements of the card.
     */
    public void Activate()
    {
        //StartCoroutine(ActivateEnum());
        cardVisuals.SpeakVisuals();
        //Invoke("InfoText", 1f);
        //InfoText();
        //TTSManager.instance.Speak(cardInfo.infoText);
    }

    private IEnumerator ActivateEnum()
    {
        cardVisuals.SpeakVisuals();
        //TTSManager.instance.Speak(cardInfo.infoText);
        yield return new WaitForSeconds(1f);
        // Maybe change voice?
        InfoText();
        //TTSManager.instance.Speak(cardInfo.infoText);
    }

    public void InfoText()
    {
        Debug.Log("Called Infotext");
        TTSManager.instance.Speak(cardInfo.infoText);

    }

    /*
     * Chose decision 1.
     */
    public void Chose1()
    {

    }

    /*
     * Chose decision 2.
     */
    public void Chose2()
    {

    }
}
