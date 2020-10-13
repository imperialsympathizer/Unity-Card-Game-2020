using System;
using System.Collections.Generic;

[Serializable]
public class CardDTO : DTO {
    public string name;
    public string description;
    public int lifeCost;
    public Card.CardRarity rarity;
    public List<DynamicEffectDTO> effects;
}