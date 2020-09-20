using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck {
    private CardStore cardStore;

    public void Initialize(Dictionary<int, Card> cardSource) {
        cardStore = new CardStore();


        // Add 5 copies of each existing card to the deck then shuffle it
        for (int i = 0; i < cardSource.Count; i++) {
            for (int j = 0; j < 5; j++) {
                Card source = cardSource[i];
                Card newCard = new Card(source, ResourceController.GenerateId());
                cardStore.AddCard(newCard);
            }
        }

        Shuffle();
    }

    public void AddCard(Card newCard) {
        // Call base class add cards method (checks are performed there)
        cardStore.AddCard(newCard);
    }

    public void AddCards(List<Card> addedCards) {
        // Call base class add cards method (checks are performed there)
        cardStore.AddCards(addedCards);
    }

    public Card DrawCard() {
        return cardStore.RemoveCard(0);
    }

    public void Shuffle() {
        cardStore.Randomize();
    }

    public int GetSize() {
        return cardStore.GetSize();
    }

    public List<Card> GetCards() {
        return cardStore.cards;
    }

    public void ClearCards() {
        cardStore.RemoveAll();
    }
}
