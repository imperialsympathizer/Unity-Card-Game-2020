using System;
using UnityEngine;

[Serializable]
public class ChangeSlots : DynamicEffect {
    public ChangeSlots(int effectCount) : base(effectCount) { }

    public override bool IsValid() {
        return (Id >= 0 && effectCount != 0);
    }

    public override void ResolveEffect() {
        if (effectCount < 0) {
            for (int i = 0; i < Mathf.Abs(effectCount); i++) {
                PlayerController.Instance.RemoveSlot();
            }
        }
        else if (effectCount > 0) {
            for (int i = 0; i < effectCount; i++) {
                PlayerController.Instance.AddSlot();
            }
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}