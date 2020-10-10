using System.Collections.Generic;

public class Deck : CardStore {
    public Deck(Dictionary<string, Card> cardSource) : base() {
        // Add 5 copies of each existing card to the deck then shuffle it
        foreach (KeyValuePair<string, Card> cardPair in cardSource) {
            for (int j = 0; j < 5; j++) {
                AddCard(new Card(cardPair.Value));
            }
        }

        Shuffle();
    }

    public Card DrawCard() {
        return RemoveCard(0);
    }

    public void Shuffle() {
        Randomize();
    }
}
