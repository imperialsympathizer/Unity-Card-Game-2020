using System.Collections.Generic;
using UnityEngine;

public class DrawCard : PlayEffect {

    public override List<Target> GetValidTargets() {
        return null;
    }

    public override void ResolveEffect() {
        Debug.Log("Drawing " + amount.ToString() + " cards.");
        for (int i = 0; i < amount; i++) {
            CardManager.SharedInstance.DrawCard();
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        EffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
