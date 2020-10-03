using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageTarget : TargetableDynamicEffect {
    private int damageValue;

    public DamageTarget(int effectCount, List<Target> validTargets, string targetingDialogue, int minTargets, int maxTargets, int damageValue) : base(effectCount, validTargets, targetingDialogue, minTargets, maxTargets) {
        this.damageValue = damageValue;
    }

    public override List<Target> GetValidTargets() {
        // TODO
        return validTargets;
    }

    public override void ResolveEffect() {
        // TODO: implement effect properly
        // Debug.Log("Dealing " + effectCount.ToString() + " damage.");
        for (int i = 0; i < effectCount; i++) {
            for (int j = 0; j < selectedTargets.Count; j++) {
                Tuple<int, Target> targetData = selectedTargets[j];
                switch (targetData.Item2) {
                    case Target.ENEMY:
                        EnemyController.UpdateLife(targetData.Item1, damageValue);
                        break;
                    case Target.SUMMON:
                        SummonController.UpdateLife(targetData.Item1, damageValue);
                        break;
                    case Target.PLAYER:
                    default:
                        PlayerController.UpdateLife(damageValue);
                        break;
                }
            }
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
