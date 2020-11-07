using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityLibrary;


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
    public static CardManager instance = null;


    //[SerializeField] private CardVisuals cardVisuals { get; set; }
    [SerializeField] private CardVisuals cardVisuals;

    //[SerializeField] private Card cardInfo { get; set; }
    [SerializeField] private Card cardInfo;

    public TMPro.TextMeshPro MainText;
    public TMPro.TextMeshPro D1Text;
    public TMPro.TextMeshPro D2Text;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void NewCard(Card newCard)
    {
        cardInfo = newCard;

        MainText.text = cardInfo.infoText;
        D1Text.text = cardInfo.decision1Text;
        D2Text.text = cardInfo.decision2Text;
        Activate();

        // update text
        // update decisions
        // update model via switch statement
        cardVisuals.NewCard(newCard.alien);
    }

    /*
     * Starts the audio and visual elements of the card.
     */
    public void Activate()
    {
        StartCoroutine(ActivateEnum());
    }

    private IEnumerator ActivateEnum()
    {
        cardVisuals.SpeakVisuals();
        yield return new WaitForSeconds(1f);
        // Maybe change voice?
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        TTSManager.instance.Say(cardInfo.infoText);
#endif
    }

    /*
     * Chose decision 1.
     */
    public void Chose1()
    {
        // Play leave animation 

        StatManager.instance.ModBiodiversity(cardInfo.d1BiodiversityModifier);
        StatManager.instance.ModHorniness(cardInfo.d1HorninessModifier);
        StatManager.instance.ModAtmosphereTemp(cardInfo.d1AtmosphereTempModifier);
        StatManager.instance.ModDomSub(cardInfo.d1DomSubModifier);

        RoommateManager.instance.AddResponse(cardInfo.d1RoommateResponse);

        DeckManager.instance.AddCardsRandom(cardInfo.d1Cards);
        DeckManager.instance.NextCard();
    }

    /*
     * Chose decision 2.
     */
    public void Chose2()
    {
        // Play leave animation 

        StatManager.instance.ModBiodiversity(cardInfo.d2BiodiversityModifier);
        StatManager.instance.ModHorniness(cardInfo.d2HorninessModifier);
        StatManager.instance.ModAtmosphereTemp(cardInfo.d2AtmosphereTempModifier);
        StatManager.instance.ModDomSub(cardInfo.d2DomSubModifier);

        RoommateManager.instance.AddResponse(cardInfo.d2RoommateResponse);

        DeckManager.instance.AddCardsRandom(cardInfo.d2Cards);
        DeckManager.instance.NextCard();
    }
}
