﻿using System.Collections.Generic;
using UnityEngine;

public class AttachEffectTarget : TargetableDynamicEffect {
    // DynamicEffect that adds the specified AttachedStaticEffect (Status or Modifier) to the target

    public AttachEffectTarget(int effectCount, List<Target> validTargets) : base(effectCount, validTargets) {
    }

    public override List<Target> GetValidTargets() {
        // TODO
        return validTargets;
    }

    public override void ResolveEffect() {
        // TODO: implement effect properly
        Debug.Log("amount:" + effectCount.ToString());
        for (int i = 0; i < effectCount; i++) {
            CardManager.SharedInstance.DrawCard();
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
