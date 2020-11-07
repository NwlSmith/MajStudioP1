using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{

    public static DeckManager instance = null;

    // The current card.
    [SerializeField]
    private Card curCard;

    // The current queue of cards.
    [SerializeField]
    private List<Card> cardList;

    // A list of miscelanious cards, not connected to any particular storyline.
    [SerializeField]
    private List<Card> miscCardList;

    // The cards the player should start with.
    [SerializeField]
    private List<Card> startingCardList;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        AddCardsRandom(startingCardList.ToArray());
        NextCard();
    }

    /*
     * Adds the card to a completely random point in the list.
     * newCard: the new card
     */
    public void AddCardRandom(Card newCard)
    {
        cardList.Insert(Random.Range(0, cardList.Count), newCard);
    }

    /*
     * Adds the card to the front of the list. It will show up next.
     * newCard: the new card
     */
    public void AddCardFront(Card newCard)
    {
        cardList.Insert(0, newCard);
    }

    /*
     * Adds the card to the back of the list. It may take a while for this card to be seen.
     * newCard: the new card
     */
    public void AddCardBack(Card newCard)
    {
        cardList.Insert(cardList.Count, newCard);
    }
    
    /*
     * Adds the card at the desired location, with a random offset if desired.
     * Card will be inserted at offset +/- Random.Range(0, randomRange)
     * offset: the index you want the new card placed at
     * newCard: the new card... obviously...
     * randomRange: the range that can be used to calculate the new card's position, default = 0.
     */
    public void AddCardAt(int offset, Card newCard, int randomRange = 0)
    {
        if (randomRange != 0)
        {
            AddCardAt(offset + Random.Range(-randomRange, randomRange), newCard, 0);
        }

        if (offset > cardList.Count)
            AddCardBack(newCard);
        else if (offset < 0)
            AddCardFront(newCard);
        else
            cardList.Insert(offset, newCard);
    }

    /*
     * Add an array of cards to the front of the list.
     */
    public void AddCardsFront(Card[] newCards)
    {
        for (int i = newCards.Length - 1; i > 1; i--)
        {
            AddCardFront(newCards[i]);
        }
    }

    /*
     * Add an array of cards to the back of the list.
     */
    public void AddCardsBack(Card[] newCards)
    {
        foreach (Card newCard in newCards)
        {
            AddCardBack(newCard);
        }
    }

    /*
     * Add an array of cards to random places in the list.
     */
    public void AddCardsRandom(Card[] newCards)
    {
        foreach (Card newCard in newCards)
        {
            cardList.Insert(Random.Range(0, cardList.Count), newCard);
        }
    }

    /*
     * Gets rid of the current card and sets the current card to the first card on the list.
     */
    public void NextCard()
    {
        if (cardList.Count <= 0)
        {
            AddCardsRandom(miscCardList.ToArray());
            AddCardsRandom(startingCardList.ToArray());
        }

        curCard = cardList[0];
        cardList.RemoveAt(0);

        CardManager.instance.NewCard(curCard);
    }
}
