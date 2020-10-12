using System.Collections.Generic;

public class AttachEffectTarget : TargetableDynamicEffect {
    // DynamicEffect that adds the specified AttachedStaticEffect (Status or Modifier) to the target

    public AttachEffectTarget(int effectCount, List<Target> validTargets, string targetingDialogue, int minTargets, int maxTargets) : base(effectCount, validTargets, targetingDialogue, minTargets, maxTargets) {}

    public override bool IsValid() {
        return (id >= 0 && effectCount >= 0 && selectedTargets != null && selectedTargets.Count > 0);
    }

    public override void ResolveEffect() {
        // TODO: implement effect properly
        for (int i = 0; i < effectCount; i++) {
            CardManager.SharedInstance.DrawCard();
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
