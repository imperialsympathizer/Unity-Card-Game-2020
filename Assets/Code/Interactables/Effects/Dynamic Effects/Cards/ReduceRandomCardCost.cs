using System.Collections.Generic;
using UnityEngine;

public class ReduceRandomCardCost : DynamicEffect {
    // This dynamic effect reduces the cost of a card in hand by reduceAmount
    // If random is true, a random card in hand will be chosen
    // Otherwise, the player chooses the card to reduce the cost

    private int reduceAmount;

    public ReduceRandomCardCost(int effectCount, int reduceAmount) : base(effectCount) {
        this.reduceAmount = reduceAmount;
    }

    public override void ResolveEffect() {
        // Will only reduce the cost of cards that don't cost 0 already
        List<Card> cards = GetNonZeroCards();
        if (cards.Count > 0) {
            for (int i = 0; i < effectCount; i++) {
                int randomIndex = Random.Range(0, cards.Count - 1);
                Card updatedCard = cards[randomIndex];
                updatedCard.UpdateLifeCost(reduceAmount);
                CardManager.SharedInstance
            }
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }

    private List<Card> GetNonZeroCards() {
        List<Card> cards = CardManager.SharedInstance.GetHandCards();
        for (int i = 0; i < cards.Count; i++) {
            if (cards[i].LifeCost == 0) {
                cards.RemoveAt(i);
            }
        }

        return cards;
    }
}