﻿using UnityEngine;

public class DrawCard : PlayEffect {
    public DrawCard(int effectCount) : base(effectCount) {
    }

    public override void ResolveEffect() {
        Debug.Log("Drawing " + effectCount.ToString() + " cards.");
        for (int i = 0; i < effectCount; i++) {
            CardManager.SharedInstance.DrawCard();
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        EffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
