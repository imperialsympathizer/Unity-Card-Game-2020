using System;
using System.Collections.Generic;

[Serializable]
public class OnTotalElementUpdate : Trigger {
    public Dictionary<Element.ElementType, int> elements;

    public OnTotalElementUpdate(List<TriggerAction> triggerActions) : base(triggerActions) {
        ElementController.OnTotalElementUpdate += OnEventTriggered;
    }

    public override void DeactivateTrigger() {
        ElementController.OnTotalElementUpdate -= OnEventTriggered;
    }

    private void OnEventTriggered(Dictionary<Element.ElementType, int> elements) {
        // Data operations
        this.elements = elements;

        if (effect != null) {
            // Call the effect with the trigger
            effect.ResolveTrigger(this);
        }
    }
}
