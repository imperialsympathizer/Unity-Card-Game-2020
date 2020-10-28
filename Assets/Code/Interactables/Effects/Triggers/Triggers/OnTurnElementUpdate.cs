using System;
using System.Collections.Generic;

[Serializable]
public class OnTurnElementUpdate : Trigger {
    public List<Element> elements;

    public OnTurnElementUpdate(List<TriggerAction> triggerActions) : base(triggerActions) {
        ElementController.OnTurnElementUpdate += OnEventTriggered;
    }

    public override void DeactivateTrigger() {
        ElementController.OnTurnElementUpdate -= OnEventTriggered;
    }

    private void OnEventTriggered(List<Element> elements) {
        // Data operations
        this.elements = elements;

        if (effect != null) {
            // Call the effect with the trigger
            effect.ResolveTrigger(this);
        }
    }
}
