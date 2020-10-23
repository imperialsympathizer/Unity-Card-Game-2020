using System.Collections.Generic;

public abstract class Modifier : AttachedStaticEffect {
    // Modifiers directly influence the value of certain parameters, such as attack on a character
    // Modifiers can only be attached to specific characters and entities, they cannot be passive

    // IMPORTANT: All modifiers, no matter what operations they are performing on the value,
    // must convert the operation to an additive value in order for calculation of values to be performed correctly

    public Modifier(int effectCount, List<Trigger> triggers, int priority) : base(effectCount, triggers, priority) { }

    protected override void RemoveEffect(Trigger trigger) {
        if (IsAttached) {
            // Clear any triggers the effect was subscribed to
            DeactivateTriggers();

            // Then, remove the effect from the character and from the StaticEffectController
            StaticEffectController.Instance.RemoveModifier(id);
        }
    }
}