﻿using System;

[Serializable]
public abstract class DynamicEffect : BaseEffect {
    // This class is to be instantiated for specific effects that are dynamically created (by card plays, attacks, etc.)
    // DynamicEffects will be pushed into the DynamicEffectController's queue and resolved in sequence
    // DynamicEffects can be owned/created by many different classes, but all effects will still be sent to the same DynamicEffectController

    // TODO: implement animations for effects so that they can be paired together
    public static event Action OnEffectComplete;

    public static new void ClearSubscriptions() {
        OnEffectComplete = null;
    }

    public DynamicEffect(int effectCount) : base(effectCount) {
    }

    public void AddBeginListener() {
        DynamicEffectController.OnEffectBegin += ResolveEffect;
    }

    public abstract bool IsValid();

    public abstract void ResolveEffect();

    protected void EffectCompleteEvent() {
        OnEffectComplete?.Invoke();
    }
}
