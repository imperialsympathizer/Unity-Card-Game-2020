using System;

[Serializable]
public class ChangeRandomFighterHealth : DynamicEffect {
    Target targetType;
    public ChangeRandomFighterHealth(int effectCount, Target targetType) : base(effectCount) {
        this.targetType = targetType;
    }

    // effectCount can be positive (heal) or negative (damage)
    // Cannot target cards
    public override bool IsValid() {
        return (Id >= 0 && effectCount != 0 && targetType != Target.CARD);
    }

    public override void ResolveEffect() {
        switch (targetType) {
            case Target.ENEMY:
                Enemy randomEnemy = EnemyController.Instance.GetRandomEnemy(true);
                if (randomEnemy != null) {
                    EnemyController.Instance.UpdateLife(randomEnemy.Id, effectCount);
                }
                break;
            case Target.SUMMON:
                Summon randomSummon = SummonController.Instance.GetRandomSummon(true);
                if (randomSummon != null) {
                    SummonController.Instance.UpdateLife(randomSummon.Id, effectCount);
                }
                break;
            case Target.PLAYER:
            default:
                PlayerController.Instance.UpdateLife(effectCount);
                break;
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
