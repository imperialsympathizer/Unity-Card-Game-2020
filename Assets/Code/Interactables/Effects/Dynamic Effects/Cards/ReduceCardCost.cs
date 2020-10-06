using System.Collections.Generic;

public class ReduceCardCost : TargetableDynamicEffect {
    // This dynamic effect reduces the cost of a card in hand by reduceAmount
    // If random is true, a random card in hand will be chosen
    // Otherwise, the player chooses the card to reduce the cost

    private int reduceAmount;
    private bool random;

    public ReduceCardCost(int effectCount, List<Target> validTargets, string targetingDialogue, int minTargets, int maxTargets, int reduceAmount, bool random = true) : base(effectCount, validTargets, targetingDialogue, minTargets, maxTargets) {
        this.reduceAmount = reduceAmount;
        this.random = random;
    }

    public override void ResolveEffect() {
        // Debug.Log("Drawing " + effectCount.ToString() + " cards.");
        for (int i = 0; i < effectCount; i++) {
            CardManager.SharedInstance.DrawCard();
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}