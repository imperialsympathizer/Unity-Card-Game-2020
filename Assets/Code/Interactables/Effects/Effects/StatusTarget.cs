using System.Collections.Generic;
using UnityEngine;

public class StatusTarget : PlayEffect {
    public List<Target> validTargets = new List<Target>();

    public override List<Target> GetValidTargets() {
        return validTargets;
    }

    public override void ResolveEffect() {
        Debug.Log("amount:" + amount.ToString());
        for (int i = 0; i < amount; i++) {
            CardManager.SharedInstance.DrawCard();
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        EffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
