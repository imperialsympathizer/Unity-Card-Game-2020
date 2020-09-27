using System.Collections.Generic;
using UnityEngine;

public class DiscardCard : PlayEffect {

    public override List<Target> GetValidTargets() {
        return null;
    }

    public override void ResolveEffect() {
        Debug.Log("amount:" + amount.ToString());
        for (int i = 0; i < amount; i++) {
            CardManager.SharedInstance.DiscardRandomCard();
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        EffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
