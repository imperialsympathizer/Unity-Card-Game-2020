using System.Collections.Generic;

public class Discard {
    private CardStore cardStore;

    public void Initialize() {
        // Initialize with 0 cards
        cardStore = new CardStore();
    }

    public void AddCard(Card newCard) {
        // Call base class add cards method (checks are performed there)
        cardStore.AddCard(newCard);
    }

    public void AddCards(List<Card> addedCards) {
        // Call base class add cards method (checks are performed there)
        cardStore.AddCards(addedCards);
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
