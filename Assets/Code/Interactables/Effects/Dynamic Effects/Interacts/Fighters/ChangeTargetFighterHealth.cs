﻿using System;
using System.Collections.Generic;

[Serializable]
public class ChangeTargetFighterHealth : TargetableDynamicEffect {
    public ChangeTargetFighterHealth(int effectCount, List<Target> validTargets, string targetingDialogue, int minTargets, int maxTargets)
        : base(effectCount, validTargets, targetingDialogue, minTargets, maxTargets) { }

    // effectCount can be positive (heal) or negative (damage)
    public override bool IsValid() {
        return (Id >= 0 && effectCount != 0 && selectedTargets != null && selectedTargets.Count > 0);
    }

    public override void ResolveEffect() {
        // TODO: implement effect properly
        // Debug.Log("Dealing " + effectCount.ToString() + " damage.");
        for (int i = 0; i < selectedTargets.Count; i++) {
            Tuple<int, Target> targetData = selectedTargets[i];
            switch (targetData.Item2) {
                case Target.ENEMY:
                    EnemyController.Instance.UpdateLife(targetData.Item1, effectCount);
                    break;
                case Target.SUMMON:
                    SummonController.Instance.UpdateLife(targetData.Item1, effectCount);
                    break;
                case Target.PLAYER:
                default:
                    PlayerController.Instance.UpdateLife(effectCount);
                    break;
            }
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
