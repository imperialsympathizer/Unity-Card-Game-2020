using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEffect {
    // This class is to be instantiated for specific effects
    // PlayEffects will be pushed into the EffectController's queue and resolved in sequence
    // PlayEffects can be owned by many different classes, but all effects will still be sent to the same EffectController

    // Current card effect ids
    // 0 - draw a card
    // 1 - discard a card
    // 2 - damage (target(s))
    // 3 - reduce attack (target(s))
    // 4 - increase summon slots
    // 5 - decrease summon slots
    // 6 - summon zombie
    public int id;
    public int amount;
    public string description;

    // TODO: implement animations for effects so that they can be paired together
    public static event Action OnEffectComplete;

    public void AddBeginListener() {
        EffectController.OnEffectBegin += ResolveEffect;
    }

    public bool IsValid() {
        bool valid = true;
        if (id < 0 || amount < 0) {
            valid = false;
        }
        return valid;
    }

    public void ResolveEffect() {
        Debug.Log("amount:" + amount.ToString());
        switch (id) {
            // Draw a card
            case 0:
                for (int i = 0; i < amount; i++) {
                    CardManager.SharedInstance.DrawCard();
                }
                break;
            // Discard a card
            case 1:
                for (int i = 0; i < amount; i++) {
                    CardManager.SharedInstance.DiscardRandomCard();
                }
                break;
            // Add slots (up to max)
            case 4:
                for (int i = 0; i < amount; i++) {
                    PlayerController.SharedInstance.AddSlot();
                }
                break;
            // Remove slots 
            case 5:
                for (int i = 0; i < amount; i++) {
                    PlayerController.SharedInstance.RemoveSlot();
                }
                break;
            // Summon a zombie
            case 6:
                for (int i = 0; i < amount; i++) {
                    SummonController.SharedInstance.CreateSummon(Summon.Summonable.ZOMBIE);
                }
                break;
            default:
                break;
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        EffectController.OnEffectBegin -= ResolveEffect;
        OnEffectComplete.Invoke();
    }
}
