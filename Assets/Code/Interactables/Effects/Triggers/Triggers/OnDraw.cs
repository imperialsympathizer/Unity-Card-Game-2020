using System;
using System.Collections.Generic;

[Serializable]
public class OnDraw : Trigger {
    public Card drawnCard;

    public OnDraw(List<TriggerAction> triggerActions) : base(triggerActions) {
        CardManager.OnCardDraw += OnEventTriggered;
    }

    public override void DeactivateTrigger() {
        CardManager.OnCardDraw -= OnEventTriggered;
    }

    private void OnEventTriggered(Card drawnCard) {
        // Data operations
        this.drawnCard = drawnCard;

        if (effect != null) {
            // Call the effect with the trigger
            effect.ResolveTrigger(this);
        }
    }
}
