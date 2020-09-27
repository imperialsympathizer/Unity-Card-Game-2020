using System.Collections.Generic;
using UnityEngine;

public class CreateSummon : PlayEffect {
    // The type of summon to create
    public Summon.Summonable summonType;

    // Summoning a monster does not target anything
    public override List<Target> GetValidTargets() {
        return null;
    }

    public override void ResolveEffect() {
        Debug.Log("Summoning " + amount.ToString() + " summons.");
        for (int i = 0; i < amount; i++) {
            SummonController.SharedInstance.CreateSummon(summonType);
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        EffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
