using System.Collections.Generic;

public class CardSource {
    public Dictionary<string, Card> allCards;

    public void InitializeCards() {
        allCards = new Dictionary<string, Card>();

        List<Card> cardList = JsonUtil.LoadCardsFromJson();
        foreach (Card card in cardList) {
            allCards.Add(card.name, card);
        }
    }
}
