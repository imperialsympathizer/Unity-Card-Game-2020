using System;
using System.Collections.Generic;

[Serializable]
public class DynamicEffectDTO : DTO {
    // All card effects will have these two fields at the very least
    // EffectType is the string name of the effect class to cast the DTO to (must be exact match)
    public string effectType;
    public int effectCount;

    // Field for summoning effects
    public Summon.Summonable summonType;

    // Fields for TargetableDynamicEffects
    public List<Target> validTargets;
    public string targetingDialogue;
    public int minTargets;
    public int maxTargets;

    // Fields for creating static effects
    public StaticEffectDTO attachedStaticEffect;
    public StaticEffectDTO passiveEffect;
}