using System.Collections.Generic;

public class AttackValue : Modifier {

    public AttackValue(Character character, int effectCount, List<EffectTiming.Trigger> timingTriggers) : base(character, ModifierType.ATK_VALUE, effectCount, timingTriggers, 0) {

    }

    public override void ResolveStaticEffect() {
    }
}
