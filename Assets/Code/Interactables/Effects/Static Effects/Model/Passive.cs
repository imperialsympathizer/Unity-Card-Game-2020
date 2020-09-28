using System.Collections.Generic;

public abstract class Passive : StaticEffect {
    // This StaticEffect child class deals specifically with effects that wouldn't be attached to one specific entity
    // Effects that do things unrelated to characters (e.g. drawing cards, reducing card costs), etc.
    // Because of how broadly applicable this class is, the brunt of the logic will be left to the inheriting child classes
    // This class could cover anything from drawing an extra card per turn to making cards cost less to skipping the next combat step, etc.

    protected PassiveType passiveType;

    public Passive(PassiveType passiveType, int effectCount, List<EffectTiming.Trigger> timingTriggers, int priority) : base(effectCount, timingTriggers, priority) {
        this.passiveType = passiveType;
    }

    public enum PassiveType {
        DRAW,
        DISCARD
    }
}
