using System.Collections.Generic;

public class DestroySummon : TargetableDynamicEffect {
    public DestroySummon(int effectCount, List<Target> validTargets, string targetingDialogue, int minTargets, int maxTargets) : base(effectCount, validTargets, targetingDialogue, minTargets, maxTargets) {
    }

    public override bool IsValid() {
        return (id >= 0 && effectCount >= 0);
    }

    public override void ResolveEffect() {
        // TODO: implement effect properly
        // Debug.Log("Dealing " + effectCount.ToString() + " damage.");
        for (int i = 0; i < effectCount; i++) {
            CardManager.Instance.DrawCard();
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
