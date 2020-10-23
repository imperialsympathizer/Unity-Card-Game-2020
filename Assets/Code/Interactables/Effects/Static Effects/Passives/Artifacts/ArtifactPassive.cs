using System.Collections.Generic;

public abstract class ArtifactPassive : Passive {
    // Artifact passives are a bit unique in that they are not directly triggered by events
    // Instead, they are controlled by the Artifact itself
    public int artifactId;

    public ArtifactPassive(int effectCount, int priority)
        : base(effectCount,
            new List<Trigger> { 
                new OnArtifactActivate(new List<Trigger.TriggerAction> { // Subscribe to artifact activate event
                    Trigger.TriggerAction.RESOLVE // Resolve the effect
                })
            },
            priority) { }
}