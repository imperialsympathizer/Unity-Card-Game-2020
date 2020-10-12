using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

[Serializable]
public class EffectDTO {
    // All card effects will have these two fields at the very least
    // EffectType is the string name of the effect class to cast the DTO to (must be exact match)
    public string EffectType;
    public int EffectCount;

    // Field for summoning effects
    // [JsonConverter(typeof(StringEnumConverter))]
    public Summon.Summonable SummonType;

    // Fields for TargetableDynamicEffects
    // [JsonConverter(typeof(StringEnumConverter))]
    public List<Target> ValidTargets;
    public string TargetingDialogue;
    public int MinTargets;
    public int MaxTargets;
}