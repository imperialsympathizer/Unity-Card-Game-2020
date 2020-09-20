using System.Collections.Generic;

public class CardSource {
    public Dictionary<int, Card> allCards;

    public void InitializeCards() {
        allCards = new Dictionary<int, Card>();

        // First card in the library
        PlayEffect newEffect = new PlayEffect();
        newEffect.id = 0;
        newEffect.amount = 2;
        newEffect.description = "draw 2 cards";
        List<PlayEffect> effects = new List<PlayEffect>();
        effects.Add(newEffect);

        Card newCard = new Card("Dark Pact", 0, "Draw 2 Cards", 5, false, effects);
        allCards.Add(0, newCard);

        // Second card in the library
        newEffect = new PlayEffect();
        newEffect.id = 0;
        newEffect.amount = 1;
        newEffect.description = "draw 1 card";
        effects = new List<PlayEffect>();
        effects.Add(newEffect);

        newCard = new Card("Not-so-dark pact", 0, "Draw 1 Card", 2, false, effects);
        allCards.Add(1, newCard);

        // Third card in the library
        newEffect = new PlayEffect();
        newEffect.id = 0;
        newEffect.amount = 3;
        newEffect.description = "draw 3 cards";
        effects = new List<PlayEffect>();
        effects.Add(newEffect);

        newCard = new Card("Got Enough Draw?", 0, "Draw 3 Cards", 10, false, effects);
        allCards.Add(2, newCard);

        // card 4
        newEffect = new PlayEffect();
        newEffect.id = 6;
        newEffect.amount = 1;
        newEffect.description = "summon 1 zombie";
        effects = new List<PlayEffect>();
        effects.Add(newEffect);

        newCard = new Card("Let's summon Mom, Edward", 0, "Summon a zombie", 10, false, effects);
        allCards.Add(3, newCard);

        // card 5
        newEffect = new PlayEffect();
        newEffect.id = 4;
        newEffect.amount = 2;
        newEffect.description = "add 2 summon slots";
        effects = new List<PlayEffect>();
        effects.Add(newEffect);

        newCard = new Card("moar portals", 0, "increase summon slots by 2", 7, false, effects);
        allCards.Add(4, newCard);

        // card 6
        newEffect = new PlayEffect();
        newEffect.id = 5;
        newEffect.amount = 1;
        newEffect.description = "remove 1 summon slot";
        effects = new List<PlayEffect>();
        effects.Add(newEffect);

        newCard = new Card("too many holes", 0, "reduce summon slots by 1", 7, false, effects);
        allCards.Add(5, newCard);
    }
}
