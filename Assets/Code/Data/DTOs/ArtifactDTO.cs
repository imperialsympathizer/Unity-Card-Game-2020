using System;
using System.Collections.Generic;

[Serializable]
public class ArtifactDTO : DTO {
    public string name;
    public string description;
    public Rarity rarity;
    public List<StaticEffectDTO> effects;
}