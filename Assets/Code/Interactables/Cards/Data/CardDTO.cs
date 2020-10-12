using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

[Serializable]
public class CardDTO {
    public string Name;
    public string Description;
    public int LifeCost;
    public Card.CardRarity Rarity;
    public List<EffectDTO> Effects;
}