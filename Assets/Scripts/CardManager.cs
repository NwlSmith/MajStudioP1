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

    [SerializeField] private GameObject d1HorninessUpArrow = null, d1HorninessDownArrow = null, d1DomSubUpArrow = null, d1DomSubDownArrow = null, d1TempUpArrow = null, d1TempDownArrow = null;
    [SerializeField] private GameObject d2HorninessUpArrow = null, d2HorninessDownArrow = null, d2DomSubUpArrow = null, d2DomSubDownArrow = null, d2TempUpArrow = null, d2TempDownArrow = null;

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
        RemoveStatChangeArrows();
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

        float delay = TTSManager.instance.Say("Incoming transmission from: " + alienName);

        yield return new WaitForSeconds(delay);

        MainText.text = "Translating...";

        cardVisuals.Activate();
        yield return new WaitForSeconds(1f);
        // Maybe change voice?
        TTSManager.instance.Say(cardInfo.infoText);
        MainText.text = cardInfo.infoText;

        D1Text.text = "Loading...";
        D2Text.text = "Loading...";

        yield return new WaitForSeconds(1f);
        D1Text.text = cardInfo.decision1Text;
        D2Text.text = cardInfo.decision2Text;
        canPressButtons = true;
        SetUpStatChangeArrows();

    }

    private void SetUpStatChangeArrows()
    { 

        Vector3 scaleMod = new Vector3(.015f, .015f, .015f);

        // DS
        if (cardInfo.d1DomSubModifier > 0)
            d1DomSubUpArrow.SetActive(true);
        else if (cardInfo.d1DomSubModifier < 0)
            d1DomSubDownArrow.SetActive(true);


        if (cardInfo.d2DomSubModifier > 0)
            d2DomSubUpArrow.SetActive(true);
        else if (cardInfo.d2DomSubModifier < 0)
            d2DomSubDownArrow.SetActive(true);

        d1DomSubUpArrow.transform.localScale = scaleMod * Mathf.Abs(cardInfo.d1DomSubModifier);
        d1DomSubDownArrow.transform.localScale = scaleMod * Mathf.Abs(cardInfo.d1DomSubModifier);
        d2DomSubUpArrow.transform.localScale = scaleMod * Mathf.Abs(cardInfo.d2DomSubModifier);
        d2DomSubDownArrow.transform.localScale = scaleMod * Mathf.Abs(cardInfo.d2DomSubModifier);

        //Temp
        if (cardInfo.d1AtmosphereTempModifier > 0)
            d1TempUpArrow.SetActive(true);
        else if (cardInfo.d1AtmosphereTempModifier < 0)
            d1TempDownArrow.SetActive(true);

        if (cardInfo.d2AtmosphereTempModifier > 0)
            d2TempUpArrow.SetActive(true);
        else if (cardInfo.d2AtmosphereTempModifier < 0)
            d2TempDownArrow.SetActive(true);

        d1TempUpArrow.transform.localScale = scaleMod * Mathf.Abs(cardInfo.d1AtmosphereTempModifier);
        d1TempDownArrow.transform.localScale = scaleMod * Mathf.Abs(cardInfo.d1AtmosphereTempModifier);
        d2TempUpArrow.transform.localScale = scaleMod * Mathf.Abs(cardInfo.d2AtmosphereTempModifier);
        d2TempDownArrow.transform.localScale = scaleMod * Mathf.Abs(cardInfo.d2AtmosphereTempModifier);


        //Horniness
        if (cardInfo.d1HorninessModifier > 0)
            d1HorninessUpArrow.SetActive(true);
        else if (cardInfo.d1HorninessModifier < 0)
            d1HorninessDownArrow.SetActive(true);

        if (cardInfo.d2HorninessModifier > 0)
            d2HorninessUpArrow.SetActive(true);
        else if (cardInfo.d2HorninessModifier < 0)
            d2HorninessDownArrow.SetActive(true);

        d1HorninessUpArrow.transform.localScale = scaleMod * Mathf.Abs(cardInfo.d1HorninessModifier);
        d1HorninessDownArrow.transform.localScale = scaleMod * Mathf.Abs(cardInfo.d1HorninessModifier);
        d2HorninessUpArrow.transform.localScale = scaleMod * Mathf.Abs(cardInfo.d2HorninessModifier);
        d2HorninessDownArrow.transform.localScale = scaleMod * Mathf.Abs(cardInfo.d2HorninessModifier);
    }

    private void RemoveStatChangeArrows()
    {
        // DS
        d1DomSubUpArrow.SetActive(false);
        d1DomSubDownArrow.SetActive(false);

        d2DomSubUpArrow.SetActive(false);
        d2DomSubDownArrow.SetActive(false);

        //Temp
        d1TempUpArrow.SetActive(false);
        d1TempDownArrow.SetActive(false);

        d2TempUpArrow.SetActive(false);
        d2TempDownArrow.SetActive(false);

        //Horniness
        d1HorninessUpArrow.SetActive(false);
        d1HorninessDownArrow.SetActive(false);

        d2HorninessUpArrow.SetActive(false);
        d2HorninessDownArrow.SetActive(false);
    }

    /*
     * Stops the audio and visual elements of the card.
     */
    public void Deactivate()
    {
        TTSManager.instance.StopSpeaking();
        NameText.text = "";
        MainText.text = "Please wait for new assignment...";
        D1Text.text = "";
        D2Text.text = "";

        RemoveStatChangeArrows();

        cardVisuals.Deactivate();
        canPressButtons = false;
    }

    /*
     * Chose decision 1.
     */
    public void Chose1()
    {
        // Play leave animation

        if (cardInfo.gameOverCard)
        {
            GameManager.instance.GameOverLose();
        }
        else
        {

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
    }

    /*
     * Chose decision 2.
     */
    public void Chose2()
    {
        // Play leave animation 
        if (cardInfo.gameOverCard)
        {
            GameManager.instance.GameOverLose();
        }
        else
        {
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
}
