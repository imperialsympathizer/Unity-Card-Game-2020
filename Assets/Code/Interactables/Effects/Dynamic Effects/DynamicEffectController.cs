using System;
using System.Collections.Generic;
using UnityEngine;

public class DynamicEffectController : MonoBehaviour {
    public static DynamicEffectController SharedInstance;
    // This class waits for dynamic effects (from cards or other sources) to be pushed onto its queue, then resolves them in sequence (FIFO)
    // effects will resolve in tandem with their animations, and the next effect will not be pushed until all previous animations are complete

    private Queue<DynamicEffect> effects = new Queue<DynamicEffect>();

    private bool effectInProgress = false;

    public static event Action OnEffectBegin;

    private void Awake() {
        SharedInstance = this;
        DynamicEffect.OnEffectComplete += OnEffectComplete;
    }

    // Will not resolve an effect until the previous effect is finished resolving
    // This is mostly for animation considerations
    void Update() {
        if (effects.Count > 0 && !effectInProgress) {
            effectInProgress = true;
            DynamicEffect effect = effects.Dequeue();
            if (effect != null) {
                Debug.Log("Resolving effect.");
                effect.AddBeginListener();
                OnEffectBegin.Invoke();
            }
            else {
                effectInProgress = false;
            }
        }
    }

    public void AddEffects(DynamicEffect[] newEffects) {
        Debug.Log("Adding effects to effect queue.");
        for (int i = 0; i < newEffects.Length; i++) {
            DynamicEffect effect = newEffects[i];
            if (effect != null && effect.IsValid()) {
                effects.Enqueue(effect);
            }
        }
    }

    public void AddEffects(List<DynamicEffect> newEffects) {
        Debug.Log("Adding effects to effect queue.");
        for (int i = 0; i < newEffects.Count; i++) {
            DynamicEffect effect = newEffects[i];
            if (effect != null && effect.IsValid()) {
                effects.Enqueue(effect);
            }
        }
    }

    private void OnEffectComplete() {
        Debug.Log("Effect completed.");
        effectInProgress = false;
    }
}
