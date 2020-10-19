using UnityEngine;

public class ChangeSlots : DynamicEffect {
    public ChangeSlots(int effectCount) : base(effectCount) { }

    public override bool IsValid() {
        return (id >= 0 && effectCount != 0);
    }

    public override void ResolveEffect() {
        if (effectCount < 0) {
            for (int i = 0; i < Mathf.Abs(effectCount); i++) {
                PlayerController.RemoveSlot();
            }
        }
        else if (effectCount > 0) {
            for (int i = 0; i < effectCount; i++) {
                PlayerController.AddSlot();
            }
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}