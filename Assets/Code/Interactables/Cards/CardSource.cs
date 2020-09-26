using System.Collections.Generic;

public class CardSource {
    public Dictionary<int, Card> allCards;

    public void InitializeCards() {
        allCards = new Dictionary<int, Card>();

        // First card in the library
        DrawCard drawEffect = new DrawCard();
        drawEffect.amount = 2;
        drawEffect.description = "draw 2 cards";
        List<PlayEffect> effects = new List<PlayEffect>();
        effects.Add(drawEffect);

        Card newCard = new Card("Dark Pact", 0, "Draw 2 Cards", 5, false, effects);
        allCards.Add(0, newCard);

        // Second card in the library
        drawEffect = new DrawCard();
        drawEffect.amount = 1;
        drawEffect.description = "draw 1 card";
        effects = new List<PlayEffect>();
        effects.Add(drawEffect);

        newCard = new Card("Not-so-dark pact", 0, "Draw 1 Card", 2, false, effects);
        allCards.Add(1, newCard);

        // Third card in the library
        drawEffect = new DrawCard();
        drawEffect.amount = 3;
        drawEffect.description = "draw 3 cards";
        effects = new List<PlayEffect>();
        effects.Add(drawEffect);

        newCard = new Card("Got Enough Draw?", 0, "Draw 3 Cards", 10, false, effects);
        allCards.Add(2, newCard);

        // card 4
        CreateSummon summonEffect = new CreateSummon();
        summonEffect.amount = 1;
        summonEffect.summonType = Summon.Summonable.ZOMBIE;
        summonEffect.description = "summon 1 zombie";
        effects = new List<PlayEffect>();
        effects.Add(summonEffect);

        newCard = new Card("Let's summon Mom, Edward", 0, "Summon a zombie", 10, false, effects);
        allCards.Add(3, newCard);

        // card 5
        ChangeSlots slotEffect = new ChangeSlots();
        slotEffect.amount = 2;
        slotEffect.addSlots = true;
        slotEffect.description = "add 2 summon slots";
        effects = new List<PlayEffect>();
        effects.Add(slotEffect);

        newCard = new Card("moar portals", 0, "increase summon slots by 2", 7, false, effects);
        allCards.Add(4, newCard);

        // card 6
        slotEffect = new ChangeSlots();
        slotEffect.amount = 1;
        slotEffect.addSlots = false;
        slotEffect.description = "remove 1 summon slot";
        effects = new List<PlayEffect>();
        effects.Add(slotEffect);

        newCard = new Card("too many holes", 0, "reduce summon slots by 1", 7, false, effects);
        allCards.Add(5, newCard);
    }
}
