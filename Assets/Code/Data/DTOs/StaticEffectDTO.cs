using System;
using System.Collections.Generic;

[Serializable]
public class StaticEffectDTO : DTO {
    // All card effects will have these two fields at the very least
    // EffectType is the string name of the effect class to cast the DTO to (must be exact match)
    public string effectType;
    public int effectCount;

    public List<TriggerDTO> triggers;

    // Field for IncrementValue Modifiers
    public int incrementTimes;

    // Field for artifact element thresholds
    public List<Element> elementsRequired;
}
