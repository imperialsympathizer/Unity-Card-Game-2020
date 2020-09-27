using System.Collections.Generic;

public abstract class Fighter {
    // abstract class defining any combatant

    // Some fighters may not be hittable (spirits?)
    public bool HasLife { get; set; }
    public int MaxLife { get; set; }
    public int LifeValue { get; set; }

    // All fighters have attack and attackTimes (even if they are zero)
    public int AttackValue { get; set; }
    public int AttackTimes { get; set; }

    // Deals with any statuses and modifiers that exist for any amount of time (e.g. not an instantaneous, 1-time effect)
    // Key - effect name
    // Value - object
    public Dictionary<string, AttachedStaticEffect> staticEffects;

    public void ResolveStaticEffects() {

    }
}