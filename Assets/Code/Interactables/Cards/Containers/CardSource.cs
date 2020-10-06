using System.Collections.Generic;

public class CardSource {
    public Dictionary<string, Card> allCards;

    public void InitializeCards() {
        allCards = new Dictionary<string, Card>();

        // First card in the library
        List<DynamicEffect> effects = new List<DynamicEffect> { new DrawCard(2) };
        Card newCard = new Card("Dark Pact", "Draw 2 Cards", 5, Card.CardRarity.COMMON, effects);
        allCards.Add(newCard.name, newCard);

        // card
        effects = new List<DynamicEffect> { new DrawCard(1) };
        newCard = new Card("Not-So-Dark Pact", "Draw 1 Card", 2, Card.CardRarity.COMMON, effects);
        allCards.Add(newCard.name, newCard);

        // card
        effects = new List<DynamicEffect> { new DrawCard(3) };
        newCard = new Card("Got Enough Draw?", "Draw 3 Cards", 7, Card.CardRarity.COMMON, effects);
        allCards.Add(newCard.name, newCard);

        // card
        effects = new List<DynamicEffect> { new CreateSummon(1, Summon.Summonable.ZOMBIE) };
        newCard = new Card("Let's summon Mom, Edward", "Summon a zombie", 5, Card.CardRarity.COMMON, effects);
        allCards.Add(newCard.name, newCard);

        // card
        effects = new List<DynamicEffect> { new ChangeSlots(2, true) };
        newCard = new Card("Moar Portals", "increase summon slots by 2", 5, Card.CardRarity.COMMON, effects);
        allCards.Add(newCard.name, newCard);

        // card
        effects = new List<DynamicEffect> { new ChangeSlots(1, false) };
        newCard = new Card("Too Many Holes", "reduce summon slots by 1", 1, Card.CardRarity.COMMON, effects);
        allCards.Add(newCard.name, newCard);

        // card
        effects = new List<DynamicEffect> { new CreateSummon(1, Summon.Summonable.SKELETON) };
        newCard = new Card("Mr. Bones' Wild Ride", "Summon a skeleton", 5, Card.CardRarity.COMMON, effects);
        allCards.Add(newCard.name, newCard);

        // card
        effects = new List<DynamicEffect> { new CreateSummon(1, Summon.Summonable.SPIRIT) };
        newCard = new Card("Spoopy Time", "Summon a spirit", 3, Card.CardRarity.COMMON, effects);
        allCards.Add(newCard.name, newCard);

        // card
        DamageTarget damageEffect = new DamageTarget(1, new List<Target> {Target.SUMMON, Target.ENEMY}, "Select 1-2 targets", 1, 2, -5);
        effects = new List<DynamicEffect> { damageEffect };
        newCard = new Card("Damage Test", "Deal 5 damage to 1 or 2 targets", 2, Card.CardRarity.COMMON, effects);
        allCards.Add(newCard.name, newCard);
    }
}
