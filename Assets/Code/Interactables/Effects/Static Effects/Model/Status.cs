using System.Collections.Generic;

public abstract class Status : AttachedStaticEffect {
    protected StatusType statusType;

    public Status(Character character, StatusType statusType, int effectCount, List<EffectTiming.Trigger> timingTriggers, int priority) : base(character, effectCount, timingTriggers, priority) {
        this.statusType = statusType;
    }

    public enum StatusType {
        INFECTION,
        UNDYING
    }
}
