using System.Collections.Generic;

public class Deck : CardStore {
    public Deck(Dictionary<string, Card> cardSource) : base() {

        if (cardSource.ContainsKey("Awaken the Bones")) {
            for (int j = 0; j < 8; j++) {
                AddCard(new Card(cardSource["Awaken the Bones"]));
            }
        }

        if (cardSource.ContainsKey("Quick Study")) {
            AddCard(new Card(cardSource["Quick Study"]));
            AddCard(new Card(cardSource["Quick Study"]));
        }

        if (cardSource.ContainsKey("Revitalize")) {
            AddCard(new Card(cardSource["Revitalize"]));
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
