public class ChangeFighterHealth : DynamicEffect {
    Fighter fighter;
    public ChangeFighterHealth(int effectCount, Fighter fighter) : base(effectCount) {
        this.fighter = fighter;
    }

    // effectCount can be positive (heal) or negative (damage)
    // Cannot target cards
    public override bool IsValid() {
        return (id >= 0 && effectCount != 0 && fighter != null);
    }

    public override void ResolveEffect() {
        if (fighter is Enemy enemy) {
            EnemyController.Instance.UpdateLife(fighter.id, effectCount);
        }
        else if (fighter is Summon summon) {
            SummonController.Instance.UpdateLife(fighter.id, effectCount);
        }
        else if (fighter is Player player) {
            PlayerController.Instance.UpdateLife(effectCount);
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
