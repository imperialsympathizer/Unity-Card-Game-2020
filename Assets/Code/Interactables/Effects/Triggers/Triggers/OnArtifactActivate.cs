using System;
using System.Collections.Generic;

[Serializable]
public class OnArtifactActivate : Trigger {
    public int artifactId;

    public OnArtifactActivate(List<TriggerAction> triggerActions) : base(triggerActions) {
        Artifact.OnArtifactActivate += OnEventTriggered;
    }

    public override void DeactivateTrigger() {
        Artifact.OnArtifactActivate -= OnEventTriggered;
    }

    private void OnEventTriggered(int artifactId) {
        // Data operations
        this.artifactId = artifactId;

        if (effect != null) {
            // Call the effect with the trigger
            effect.ResolveTrigger(this);
        }
    }
}
