﻿using System.Collections.Generic;
public class Cursed : Status {
    public Cursed(int effectCount)
        : base(
            effectCount,
            new List<Trigger> { // List of triggers for the effect
                new OnEffectCountChange(new List<Trigger.TriggerAction> { // Subscribe to the OnEffectCountChange trigger
                    Trigger.TriggerAction.RESOLVE // Execute ResolveEffect() when triggered
                })
            },
            0) { }

    protected override void OperateOnEffect(Trigger trigger) { }

    protected override void ResolveEffect(Trigger trigger) {
        DynamicEffectController.SharedInstance.AddEffect(new ChangeRandomFighterHealth(-effectCount, Target.ENEMY));
    }
}