using System.Collections.Generic;

public abstract class Modifier : AttachedStaticEffect {
    // Modifiers directly influence the value of certain parameters, such as attack on a character
    // Modifiers can only be attached to specific characters and entities, they cannot be passive
    protected ModifierType modifierType;

    public Modifier(Character character, ModifierType modifierType, int effectCount, List<EffectTiming.Trigger> timingTriggers, int priority) : base(character, effectCount, timingTriggers, priority) {
        this.modifierType = modifierType;
    }

    public enum ModifierType {
        ATK_VALUE,
        ATK_TIMES,
        MAX_LIFE,
        LIFE_VALUE,
        MAX_WILL,
        WILL_VALUE
    }
}