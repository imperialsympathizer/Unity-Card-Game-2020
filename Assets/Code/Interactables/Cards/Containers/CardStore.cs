﻿using System.Collections.Generic;

public abstract class CardStore {
    // Generic class for handling cards that are not currently in the hand
    // Other locations where cards are kept (deck or discard for example) should inherit this class for simplified card management
    protected List<Card> cards = new List<Card>();

    public CardStore() { }

    protected void Randomize() {
        int n = cards.Count;
        while (n > 1) {
            n--;
            int r = RandomNumberGenerator.Instance.GetRandomIntFromRange(n + 1);
            Card c = cards[r];
            cards[r] = cards[n];
            cards[n] = c;
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

    public List<Card> GetCards() {
        return cards;
    }
}
