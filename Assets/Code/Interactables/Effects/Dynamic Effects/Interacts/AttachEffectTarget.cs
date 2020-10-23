using System;
using System.Collections.Generic;

public class AttachEffectTarget : TargetableDynamicEffect {
    // DynamicEffect that adds the specified AttachedStaticEffect (Status or Modifier) to the target
    AttachedStaticEffect attachedEffect;

    public AttachEffectTarget(int effectCount,
        List<Target> validTargets,
        string targetingDialogue,
        int minTargets,
        int maxTargets,
        AttachedStaticEffect attachedStaticEffect)
        : base(effectCount, validTargets, targetingDialogue, minTargets, maxTargets) {
        if (attachedStaticEffect != null) {
            this.attachedEffect = attachedStaticEffect;
        }
        else {
            throw new Exception("Cannot create AttachEffectTarget effect without a StaticEffect to attach.");
        }
    }

    public override bool IsValid() {
        return (id >= 0 && effectCount >= 0 && selectedTargets != null && selectedTargets.Count > 0);
    }

    public override void ResolveEffect() {
        // TODO: implement effect properly
        for (int i = 0; i < selectedTargets.Count; i++) {
            Tuple<int, Target> targetData = selectedTargets[i];
            switch (targetData.Item2) {
                case Target.ENEMY:
                    AddEffect(EnemyController.Instance.GetEnemy(targetData.Item1));
                    break;
                case Target.SUMMON:
                    AddEffect(SummonController.Instance.GetSummon(targetData.Item1));
                    break;
                case Target.PLAYER:
                default:
                    AddEffect(PlayerController.Instance.GetPlayer());
                    break;
            }
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }

    private void AddEffect(Character target) {
        bool attached = false;
        foreach (KeyValuePair<int, AttachedStaticEffect> targetEffect in target.attachedEffects) {
            if (targetEffect.Value.GetType() == attachedEffect.GetType()) {
                targetEffect.Value.UpdateEffectCount(attachedEffect.effectCount);
                target.attachedEffects[targetEffect.Key] = targetEffect.Value;
                attached = true;
                break;
            }
        }
        if (!attached) {
            if (attachedEffect is Modifier modifier) {
                StaticEffectController.Instance.AddModifier(target, modifier);
            }
            else if (attachedEffect is Status status) {
                StaticEffectController.Instance.AddStatus(target, status);
            }
        }
    }
}
