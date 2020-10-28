using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AlienEnum { Scientist, Undercover, Assimilation, Corporate, Clown, Vacation, Artist, Lonely };

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    [Header("Information")]
    // The main info of the card, ie, what the scenario is.
    [TextArea(10, 100)]
    public string infoText;
    // Which alien is this? either Scientist, Undercover, Assimilation, Corporate, Clown, Vacation, Artist, Lonely
    //public string model = "Placeholder";
    public AlienEnum alien;

    [Header("Decision 1")]
    // Text on the first decision's button
    [TextArea(10, 100)]
    public string decision1Text;
    // Modifiers added or subtracted from various stats
    public int d1BiodiversityModifier = 0;
    public int d1HorninessModifier = 0;
    public int d1AtmosphereTempModifier = 0;
    public int d1DomSubModifier = 0;

    // The response of your roommate
    public string d1RoommateResponse;

    // Cards to be shuffled into deck for choosing d1
    public Card[] d1Cards;

    [Header("Decision 2")]
    // Text on the second decision's button
    [TextArea(10, 100)]
    public string decision2Text;
    // Modifiers added or subtracted from various stats
    public int d2BiodiversityModifier = 0;
    public int d2HorninessModifier = 0;
    public int d2AtmosphereTempModifier = 0;
    public int d2DomSubModifier = 0;

    // The response of your roommate
    public string d2RoommateResponse;

    // Cards to be shuffled into deck for choosing d2
    public Card[] d2Cards;
}
