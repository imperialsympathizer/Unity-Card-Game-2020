﻿using UnityEngine;

public class CreatePassiveEffect : DynamicEffect {
    // DynamicEffect that creates the specified PassiveStaticEffect (Status only)
    // These are specifically for effects that do not target

    public CreatePassiveEffect(int effectCount) : base(effectCount) {
    }

    public override void ResolveEffect() {
        Debug.Log("amount:" + effectCount.ToString());
        for (int i = 0; i < effectCount; i++) {
            CardManager.SharedInstance.DrawCard();
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
