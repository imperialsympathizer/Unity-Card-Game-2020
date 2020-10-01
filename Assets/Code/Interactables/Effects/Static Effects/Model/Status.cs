using System.Collections.Generic;

public abstract class Status : AttachedStaticEffect {
    // Statuses tend to be more "special" effects that often require unique implementation
    // Statuses can only be attached to specific characters and entities, they cannot be passive
    public readonly StatusType statusType;

    public Status(Character character, StatusType statusType, int effectCount, List<Trigger> triggers, int priority) : base(character, effectCount, triggers, priority) {
        this.statusType = statusType;
    }

    public enum StatusType {
        INFECTOR,       // Status native to zombies, add infection status to any enemies that it attacks or is killed by
        INFECTION,
        UNDYING
    }

    protected override void RemoveEffect(Trigger trigger) {
        // Clear any triggers the effect was subscribed to
        for (int i = 0; i < Triggers.Count; i++) {
            Triggers[i].DeactivateTrigger();
        }

        // Then, remove the effect from the character and from the StaticEffectController
        StaticEffectController.RemoveStatus(id);
    }
}
