public class ChangeFighterAttackValue : DynamicEffect {
    private Fighter fighter;

    public ChangeFighterAttackValue(int effectCount, Fighter fighter) : base(effectCount) {
        this.fighter = fighter;
    }

    // effectCount can be positive or negative
    // Cannot target cards
    public override bool IsValid() {
        return (id >= 0 && effectCount != 0 && fighter != null);
    }

    public override void ResolveEffect() {
        fighter.UpdateAttackTimes(effectCount);

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}