using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    [SerializeField] private CardVisuals cardVisuals = null;

    //[SerializeField] private Card cardInfo { get; set; }
    [SerializeField] private Card cardInfo = null;

    public TMPro.TextMeshPro MainText;
    public TMPro.TextMeshPro D1Text;
    public TMPro.TextMeshPro D2Text;
    public TextMesh NameText;

    public bool canPressButtons = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        NameText.text = "";
        MainText.text = "";
        D1Text.text = "";
        D2Text.text = "";
    }

    public void NewCard(Card newCard)
    {
        cardInfo = newCard;
        cardVisuals.NewCard(newCard.alien);

        Activate();

        // update text
        // update decisions
        // update model via switch statement
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
        string alienName = "";
        switch (cardInfo.alien)
        {
            case AlienEnum.Scientist:
                alienName = "Dr. Elglore Smankoff";
                break;
            case AlienEnum.Undercover:
                alienName = "Torbar Contor";
                break;
            case AlienEnum.Assimilation:
                alienName = "S'SSS SSSSS";
                break;
            case AlienEnum.Corporate:
                alienName = "Smarglarff Sneet";
                break;
            case AlienEnum.Clown:
                alienName = "Bloof Gloof";
                break;
            case AlienEnum.Vacation:
                alienName = "Zem Lips";
                break;
            case AlienEnum.Artist:
                alienName = "Goya Carr";
                break;
            case AlienEnum.Lonely:
                alienName = "Mire Oodle";
                break;
        }
        NameText.text = alienName;
       
        MainText.text = "Translating...";

        cardVisuals.Activate();
        yield return new WaitForSeconds(1f);
        // Maybe change voice?
        UnityWebGLSpeechSynthesis.TTSManager.instance.Say(cardInfo.infoText, 1); // FIX THIS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        MainText.text = cardInfo.infoText;

        D1Text.text = "Loading...";
        D2Text.text = "Loading...";

        yield return new WaitForSeconds(1f);
        D1Text.text = cardInfo.decision1Text;
        D2Text.text = cardInfo.decision2Text;
        canPressButtons = true;
    }

    /*
     * Stops the audio and visual elements of the card.
     */
    public void Deactivate()
    {
        NameText.text = "";
        MainText.text = "Please wait for new assignment...";
        D1Text.text = "";
        D2Text.text = "";

        cardVisuals.Deactivate();
        canPressButtons = false;
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

        RoommateManager.instance.RoommateResponse(cardInfo, true);

        DeckManager.instance.AddCardsRandom(cardInfo.d1Cards);

        if (cardInfo.imageAssociatedWithChoice1 && cardInfo.image != null)
        {
            TVManager.instance.NewImage(cardInfo.image);
        }

        Deactivate();
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

        RoommateManager.instance.RoommateResponse(cardInfo, false);

        DeckManager.instance.AddCardsRandom(cardInfo.d2Cards);

        if (!cardInfo.imageAssociatedWithChoice1 && cardInfo.image != null)
        {
            TVManager.instance.NewImage(cardInfo.image);
        }

        Deactivate();
    }
}
