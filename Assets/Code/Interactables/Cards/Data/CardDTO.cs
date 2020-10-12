using System;
using System.Collections.Generic;

[Serializable]
public class CardDTO {
    public string Name;
    public string Description;
    public int LifeCost;
    public string Rarity;
    public List<EffectDTO> Effects;
}