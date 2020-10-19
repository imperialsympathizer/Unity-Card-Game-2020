﻿public class ChangeRandomFighterHealth : DynamicEffect {
    Target targetType;
    public ChangeRandomFighterHealth(int effectCount, Target targetType) : base(effectCount) {
        this.targetType = targetType;
    }

    // effectCount can be positive (heal) or negative (damage)
    // Cannot target cards
    public override bool IsValid() {
        return (id >= 0 && effectCount != 0 && targetType != Target.CARD);
    }

    public override void ResolveEffect() {
        switch (targetType) {
            case Target.ENEMY:
                Enemy randomEnemy = EnemyController.GetRandomEnemy(true);
                if (randomEnemy != null) {
                    EnemyController.UpdateLife(randomEnemy.id, effectCount);
                }
                break;
            case Target.SUMMON:
                Summon randomSummon = SummonController.GetRandomSummon(true);
                if (randomSummon != null) {
                    SummonController.UpdateLife(randomSummon.id, effectCount);
                }
                break;
            case Target.PLAYER:
            default:
                PlayerController.UpdateLife(effectCount);
                break;
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}