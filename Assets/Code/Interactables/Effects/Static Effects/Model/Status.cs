using System.Collections.Generic;

public abstract class Status : AttachedStaticEffect {
    // Statuses tend to be more "special" effects that often require unique implementation
    // Statuses can only be attached to specific characters and entities, they cannot be passive
    public readonly StatusType statusType;

    public Status(Character character, StatusType statusType, int effectCount, Dictionary<TriggerAction.Trigger, TriggerAction> triggerActions, int priority) : base(character, effectCount, triggerActions, priority) {
        this.statusType = statusType;
    }

    public enum StatusType {
        INFECTOR,       // Status native to zombies, add infection status to any enemies that it attacks or is killed by
        INFECTION,
        UNDYING
    }

    protected override void RemoveEffect() {
        StaticEffectController.SharedInstance.RemoveStatus(this, character);
    }
}
