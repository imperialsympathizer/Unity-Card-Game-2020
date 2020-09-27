using UnityEngine;

public class ChangeSlots : PlayEffect {
    // determines whether the effect is adding (true) or removing (false) slots
    private readonly bool addSlots = true;

    public ChangeSlots(int repeatCount, bool addSlots) : base(repeatCount) {
        this.addSlots = addSlots;
    }

    public override void ResolveEffect() {
        if (addSlots) {
            Debug.Log("Adding " + repeatCount.ToString() + " slots.");
            for (int i = 0; i < repeatCount; i++) {
                PlayerController.SharedInstance.AddSlot();
            }
        }
        else {
            Debug.Log("Removing " + repeatCount.ToString() + " slots.");
            for (int i = 0; i < repeatCount; i++) {
                PlayerController.SharedInstance.RemoveSlot();
            }
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        EffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}