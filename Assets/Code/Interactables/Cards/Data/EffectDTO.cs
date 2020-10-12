﻿using System;
using System.Collections.Generic;

[Serializable]
public class EffectDTO {
    // All card effects will have these two fields at the very least
    // EffectType is the string name of the effect class to cast the DTO to (must be exact match)
    public string EffectType;
    public int EffectCount;

    // Field for summoning effects
    // public Summon.Summonable Summonable { get; set; }

    // Fields for TargetableDynamicEffects
    // public List<Target> ValidTargets { get; set; }
    public string TargetingDialogue;
    public int MinTargets;
    public int MaxTargets;
}