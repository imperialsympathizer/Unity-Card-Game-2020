using System;
using System.Collections.Generic;

public abstract class PlayEffect {
    // This class is to be instantiated for specific effects
    // PlayEffects will be pushed into the EffectController's queue and resolved in sequence
    // PlayEffects can be owned by many different classes, but all effects will still be sent to the same EffectController
    public int id;

    // Number of times to directly repeat this effect in a row.
    protected int repeatCount;

    // TODO: implement animations for effects so that they can be paired together
    public static event Action OnEffectComplete;

    public PlayEffect(int repeatCount) {
        this.repeatCount = repeatCount;
    }

    public void AddBeginListener() {
        EffectController.OnEffectBegin += ResolveEffect;
    }

    public bool IsValid() {
        if (id < 0 || repeatCount < 0) {
            return false;
        }
        return true;
    }

    public abstract void ResolveEffect();

    protected void EffectCompleteEvent() {
        OnEffectComplete?.Invoke();
    }
}
