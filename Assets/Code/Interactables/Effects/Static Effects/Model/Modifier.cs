using System.Collections.Generic;

public abstract class Modifier : AttachedStaticEffect {
    // Modifiers directly influence the value of certain parameters, such as attack on a character
    // Modifiers can only be attached to specific characters and entities, they cannot be passive
    public readonly ModifierType modifierType;

    // IMPORTANT: All modifiers, no matter what operations they are performing on the value, must convert the operation to an additive value in order for calculation of values to be performed correctly

    public Modifier(Character character, ModifierType modifierType, int effectCount, List<Trigger> triggers, int priority) : base(character, effectCount, triggers, priority) {
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

    protected override void RemoveEffect(Trigger trigger) {
        // Clear any triggers the effect was subscribed to
        for (int i = 0; i < Triggers.Count; i++) {
            Triggers[i].DeactivateTrigger();
        }

        // Then, remove the effect from the character and from the StaticEffectController
        StaticEffectController.RemoveModifier(id);
    }
}