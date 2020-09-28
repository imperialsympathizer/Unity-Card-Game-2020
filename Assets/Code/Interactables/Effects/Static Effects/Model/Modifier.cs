using System.Collections.Generic;

public abstract class Modifier : AttachedStaticEffect {
    // Modifiers directly influence the value of certain parameters, such as attack on a character
    // Modifiers can only be attached to specific characters and entities, they cannot be passive
    public readonly ModifierType modifierType;

    // IMPORTANT: All modifiers, no matter what operations they are performing on the value, must convert the operation to an additive value in order for calculation of values to be performed correctly

    public Modifier(Character character, ModifierType modifierType, int effectCount, Dictionary<TriggerAction.Trigger, TriggerAction> triggerActions, int priority) : base(character, effectCount, triggerActions, priority) {
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

    protected override void RemoveEffect() {
        StaticEffectController.SharedInstance.RemoveModifier(this, character);
    }
}