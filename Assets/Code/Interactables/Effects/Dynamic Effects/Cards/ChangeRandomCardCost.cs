﻿using System.Collections.Generic;
using UnityEngine;

public class ChangeRandomCardCost : DynamicEffect {
    // This dynamic effect reduces the cost of a card in hand by effectCount
    public ChangeRandomCardCost(int effectCount) : base(effectCount) {}

    public override bool IsValid() {
        return (id >= 0 && effectCount != 0);
    }

    public override void ResolveEffect() {
        List<Card> cards;
        if (effectCount < 0) {
            // Will only reduce the cost of cards that don't cost 0 already
            cards = GetNonZeroCards();
        }
        else {
            cards = CardManager.SharedInstance.GetHandCards();
        }

        if (cards.Count > 0) {
            int randomIndex = Random.Range(0, cards.Count - 1);
            Card updatedCard = cards[randomIndex];
            updatedCard.UpdateLifeCost(effectCount);
            CardManager.SharedInstance.UpdateHandCard(updatedCard);
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