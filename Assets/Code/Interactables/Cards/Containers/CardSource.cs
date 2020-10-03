using System.Collections.Generic;

public class CardSource {
    public Dictionary<int, Card> allCards;

    public void InitializeCards() {
        allCards = new Dictionary<int, Card>();

        // First card in the library
        DrawCard drawEffect = new DrawCard(2);
        List<DynamicEffect> effects = new List<DynamicEffect>();
        effects.Add(drawEffect);

        Card newCard = new Card("Dark Pact", 0, "Draw 2 Cards", 5, effects);
        allCards.Add(0, newCard);

        // Second card in the library
        drawEffect = new DrawCard(1);
        effects = new List<DynamicEffect>();
        effects.Add(drawEffect);

        newCard = new Card("Not-so-dark pact", 0, "Draw 1 Card", 2, effects);
        allCards.Add(1, newCard);

        // Third card in the library
        drawEffect = new DrawCard(3);
        effects = new List<DynamicEffect>();
        effects.Add(drawEffect);

        newCard = new Card("Got Enough Draw?", 0, "Draw 3 Cards", 5, effects);
        allCards.Add(2, newCard);

        // card 4
        CreateSummon summonEffect = new CreateSummon(1, Summon.Summonable.ZOMBIE);
        effects = new List<DynamicEffect>();
        effects.Add(summonEffect);

        newCard = new Card("Let's summon Mom, Edward", 0, "Summon a zombie", 10, effects);
        allCards.Add(3, newCard);

        // card 5
        ChangeSlots slotEffect = new ChangeSlots(2, true);
        effects = new List<DynamicEffect>();
        effects.Add(slotEffect);

        newCard = new Card("moar portals", 0, "increase summon slots by 2", 7, effects);
        allCards.Add(4, newCard);

        // card 6
        slotEffect = new ChangeSlots(1, false);
        effects = new List<DynamicEffect>();
        effects.Add(slotEffect);

        newCard = new Card("too many holes", 0, "reduce summon slots by 1", 7, effects);
        allCards.Add(5, newCard);

        // card 7
        summonEffect = new CreateSummon(1, Summon.Summonable.SKELETON);
        effects = new List<DynamicEffect>();
        effects.Add(summonEffect);

        newCard = new Card("Mr. Bones' Wild Ride", 0, "Summon a skeleton", 5, effects);
        allCards.Add(6, newCard);

        // card 8
        summonEffect = new CreateSummon(1, Summon.Summonable.SPIRIT);
        effects = new List<DynamicEffect>();
        effects.Add(summonEffect);

        newCard = new Card("Spooky time", 0, "Summon a spirit", 6, effects);
        allCards.Add(7, newCard);

        // card 9
        DamageTarget damageEffect = new DamageTarget(1, new List<Target> {Target.SUMMON, Target.ENEMY}, "Select 1-2 targets", 1, 2, -5);
        effects = new List<DynamicEffect> {damageEffect};

        newCard = new Card("Damage test", 0, "Deal 5 damage to 1 or 2 targets", 3, effects);
        allCards.Add(8, newCard);
    }
}
