using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSlots : PlayEffect {
    // determines whether the effect is adding (true) or removing (false) slots
    public bool addSlots = true;

    public override List<Target> GetValidTargets() {
        return null;
    }

    public override void ResolveEffect() {
        if (addSlots) {
            Debug.Log("Adding " + amount.ToString() + " slots.");
            for (int i = 0; i < amount; i++) {
                PlayerController.SharedInstance.AddSlot();
            }
        }
        else {
            Debug.Log("Removing " + amount.ToString() + " slots.");
            for (int i = 0; i < amount; i++) {
                PlayerController.SharedInstance.RemoveSlot();
            }
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        EffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}