using UnityEngine;

public class ChangeSlots : PlayEffect {
    // determines whether the effect is adding (true) or removing (false) slots
    private readonly bool addSlots = true;

    public ChangeSlots(int effectCount, bool addSlots) : base(effectCount) {
        this.addSlots = addSlots;
    }

    public override void ResolveEffect() {
        if (addSlots) {
            Debug.Log("Adding " + effectCount.ToString() + " slots.");
            for (int i = 0; i < effectCount; i++) {
                PlayerController.SharedInstance.AddSlot();
            }
        }
        else {
            Debug.Log("Removing " + effectCount.ToString() + " slots.");
            for (int i = 0; i < effectCount; i++) {
                PlayerController.SharedInstance.RemoveSlot();
            }
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        EffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}