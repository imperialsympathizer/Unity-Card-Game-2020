using System.Collections.Generic;

public class ReduceCardCost : TargetableDynamicEffect {
    // This dynamic effect reduces the cost of a card in hand by reduceAmount
    // If random is true, a random card in hand will be chosen
    // Otherwise, the player chooses the card to reduce the cost

    private int reduceAmount;
    private bool random;

    public ReduceCardCost(int effectCount,
        string targetingDialogue,
        int minTargets,
        int maxTargets,
        int reduceAmount)
        : base(effectCount, new List<Target> { Target.CARD }, targetingDialogue, minTargets, maxTargets) {
        this.reduceAmount = reduceAmount;
    }

    public override void ResolveEffect() {
        // Debug.Log("Drawing " + effectCount.ToString() + " cards.");
        for (int i = 0; i < effectCount; i++) {
            for (int j = 0; j < selectedTargets.Count; j++) {

            }
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}