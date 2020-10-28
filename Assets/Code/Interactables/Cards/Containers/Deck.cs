using System.Collections.Generic;

public class Deck : CardStore {
    public Deck(Dictionary<string, Card> cardSource) : base() {
        Card card;
        if (cardSource.TryGetValue("Awaken the Bones", out card) && card != null) {
            for (int j = 0; j < 8; j++) {
                AddCard(new Card(card));
            }
        }

        if (cardSource.TryGetValue("Quick Study", out card) && card != null) {
            AddCard(new Card(card));
            AddCard(new Card(card));
        }

        if (cardSource.TryGetValue("Revitalize", out card) && card != null) {
            AddCard(new Card(card));
        }
    }

    // Constructor meant for copying the run deck but keeping the same card ids
    // This allows for permanently changing cards in the run deck during battle by explicitly editing the card in the runDeck with the same id
    public Deck(Deck deckToCopy) : base() {
        foreach (Card card in deckToCopy.cards) {
            AddCard(new Card(card, true));
        }
    }

    public Card DrawCard() {
        return RemoveCard(0);
    }

    public void Shuffle() {
        Randomize();
    }
}
