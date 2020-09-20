using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStore {
    // Generic class for handling cards that are not currently in the hand
    // Other locations where cards are kept (deck or discard for example) should inherit this class for simplified card management

    public List<Card> cards = new List<Card>();

    public void Randomize() {
        System.Random rnd = new System.Random();
        int n = cards.Count;
        for (int i = 0; i < cards.Count - 1; i++) {
            int r = i + rnd.Next(n - i);
            Card c = cards[r];
            cards[r] = cards[i];
            cards[i] = c;
        }
    }

    public void AddCard(Card newCard) {
        // ensure no visual objects are assigned before adding
        if (newCard != null) {
            newCard.ClearVisual();
            cards.Add(newCard);
        }
    }

    public void AddCards(List<Card> addedCards) {
        // ensure no visual objects are assigned before adding
        for (int i = 0; i < addedCards.Count; i++) {
            Card newCard = addedCards[i];
            if (newCard != null) {
                newCard.ClearVisual();
                cards.Add(newCard);
            }
        }
    }

    public Card RemoveCard(int index) {
        if (index < cards.Count) {
            Card removedCard = cards[index];
            cards.RemoveAt(index);
            return removedCard;
        }

        return null;
    }

    public List<Card> RemoveAll() {
        if (cards.Count > 0) {
            List<Card> removedCards = cards;
            cards = new List<Card>();
            return removedCards;
        }

        return new List<Card>();
    }

    public int GetSize() {
        return cards.Count;
    }
}
