using System.Collections.Generic;

public class Hand {
    // Dictionary of ids (key) and Cards (value)
    private List<Card> cards = new List<Card>();

    // TODO: Set to a global variable
    private int maxSize = 10;

    public void AddCard(Card card) {
        cards.Add(card); 
    }

    public void UpdateCard(Card card) {
        for (int i = 0; i < cards.Count; i++) {
            if (cards[i].Id == card.Id) {
                cards[i] = card;
                break;
            }
        }
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

    public Card GetCard(int cardId) {
        for (int i = 0; i < cards.Count; i++) {
            if (cards[i].Id == cardId) {
                return cards[i];
            }
        }

        return null;
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
