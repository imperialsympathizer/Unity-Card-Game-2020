using System.Collections.Generic;

public abstract class Status : AttachedStaticEffect {
    // Statuses tend to be more "special" effects that often require unique implementation
    // Statuses can only be attached to specific characters and entities, they cannot be passive
    protected StatusType statusType;

    public Status(Character character, StatusType statusType, int effectCount, List<EffectTiming.Trigger> timingTriggers, int priority) : base(character, effectCount, timingTriggers, priority) {
        this.statusType = statusType;
    }

    public enum StatusType {
        INFECTION,
        UNDYING
    }
}
