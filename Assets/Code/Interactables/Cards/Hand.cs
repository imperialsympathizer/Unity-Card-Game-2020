using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand {

    // Dictionary of ids (key) and Cards (value)
    private List<Card> cards = new List<Card>();

    // TODO: Set to a global variable
    private int maxSize = 10;

    public void AddCard(Card card) {
        cards.Add(card); 
    }

    public Card RemoveCard(int cardId) {
        Card removed = null;
        for (int i = 0; i < cards.Count; i++) {
            removed = cards[i];
            if (removed.Id == cardId) {
                cards.RemoveAt(i);
                break;
            }
            else {
                removed = null;
            }
        }

        return removed;
    }

    public List<Card> GetCards() {
        return cards;
    }

    public void ClearHand() {
        cards.Clear();
    }

    public int GetSize() {
        return cards.Count;
    }

    public bool CanDraw() {
        return cards.Count < maxSize;
    }
}
