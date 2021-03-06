﻿using System;

[Serializable]
public class ChangeFighterAttackValue : DynamicEffect {
    // IMPORTANT: this class should only be used programmatically at runtime
    private Fighter fighter;

    public ChangeFighterAttackValue(int effectCount, Fighter fighter) : base(effectCount) {
        this.fighter = fighter;
    }

    // effectCount can be positive or negative
    // Cannot target cards
    public override bool IsValid() {
        return (Id >= 0 && effectCount != 0 && fighter != null);
    }

    public override void ResolveEffect() {
        fighter.UpdateAttackValue(effectCount);
        fighter.UpdateVisual();

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}