using System.Collections.Generic;

public class Deck : CardStore {
    public Deck(Dictionary<int, Card> cardSource) : base() {
        // Add 5 copies of each existing card to the deck then shuffle it
        for (int i = 0; i < cardSource.Count; i++) {
            for (int j = 0; j < 5; j++) {
                Card source = cardSource[i];
                Card newCard = new Card(source);
                AddCard(newCard);
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
