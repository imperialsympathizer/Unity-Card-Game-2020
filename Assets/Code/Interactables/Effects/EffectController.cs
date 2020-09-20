using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectController : MonoBehaviour {
    public static EffectController SharedInstance;
    // This class waits for effects (from cards or other sources) to be pushed onto its queue, then resolves them in sequence (FIFO)
    // effects will resolve in tandem with their animations, and the next effect will not be pushed until all previous animations are complete

    private Queue<PlayEffect> effects = new Queue<PlayEffect>();

    private bool effectInProgress = false;

    public static event Action OnEffectBegin;

    private void Awake() {
        SharedInstance = this;
        PlayEffect.OnEffectComplete += OnEffectComplete;
    }

    // Will not resolve an effect until the previous effect is finished resolving
    // This is mostly for animation considerations
    void Update() {
        if (effects.Count > 0 && !effectInProgress) {
            effectInProgress = true;
            PlayEffect effect = effects.Dequeue();
            if (effect != null) {
                effect.AddBeginListener();
                OnEffectBegin.Invoke();
            }
            else {
                effectInProgress = false;
            }
        }
    }

    public void AddEffects(PlayEffect[] newEffects) {
        for (int i = 0; i < newEffects.Length; i++) {
            PlayEffect effect = newEffects[i];
            if (effect != null && effect.IsValid()) {
                effects.Enqueue(effect);
            }
        }
    }

    public void AddEffects(List<PlayEffect> newEffects) {
        for (int i = 0; i < newEffects.Count; i++) {
            PlayEffect effect = newEffects[i];
            if (effect != null && effect.IsValid()) {
                effects.Enqueue(effect);
            }
        }
    }

    private void OnEffectComplete() {
        effectInProgress = false;
    }
}
