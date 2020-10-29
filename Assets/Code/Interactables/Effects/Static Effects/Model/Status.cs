using System;
using System.Collections.Generic;

[Serializable]
public abstract class Status : AttachedStaticEffect {
    // Statuses tend to be more "special" effects that often require unique implementation
    // Statuses can only be attached to specific characters and entities, they cannot be passive

    public Status(int effectCount, List<Trigger> triggers, int priority) : base(effectCount, triggers, priority) { }

    protected override void RemoveEffect(Trigger trigger) {
        if (IsAttached) {
            // Clear any triggers the effect was subscribed to
            DeactivateTriggers();

            // Then, remove the effect from the character and from the StaticEffectController
            StaticEffectController.Instance.RemoveStatus(Id);
        }
    }
}
