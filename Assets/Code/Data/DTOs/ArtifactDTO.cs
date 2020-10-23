using System;
using System.Collections.Generic;

[Serializable]
public class ArtifactDTO : DTO {
    public string name;
    public string description;
    public Rarity rarity;
    public List<StaticEffectDTO> effects;
    public bool controlledByTurnElements;

    // Field for artifact element thresholds
    // This will be automatically copied to every passive effect contained on the artifact
    public List<Element> elementsRequired;
}