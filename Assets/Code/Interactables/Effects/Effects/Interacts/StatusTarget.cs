using System.Collections.Generic;
using UnityEngine;

public class StatusTarget : TargetablePlayEffect {
    public StatusTarget(int repeatCount, List<Target> validTargets) : base(repeatCount, validTargets) {
    }

    public override List<Target> GetValidTargets() {
        // TODO
        return validTargets;
    }

    public override void ResolveEffect() {
        Debug.Log("amount:" + repeatCount.ToString());
        for (int i = 0; i < repeatCount; i++) {
            CardManager.SharedInstance.DrawCard();
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        EffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
