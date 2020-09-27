using System.Collections.Generic;
using UnityEngine;

public class DestroySummon : TargetablePlayEffect {
    public DestroySummon(int repeatCount, List<Target> validTargets) : base(repeatCount, validTargets) {
    }

    public override List<Target> GetValidTargets() {
        // TODO
        return validTargets;
    }

    public override void ResolveEffect() {
        Debug.Log("Dealing " + repeatCount.ToString() + " damage.");
        for (int i = 0; i < repeatCount; i++) {
            CardManager.SharedInstance.DrawCard();
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        EffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
