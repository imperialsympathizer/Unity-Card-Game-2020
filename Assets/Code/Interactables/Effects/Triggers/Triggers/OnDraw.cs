﻿using System.Collections.Generic;

public class OnDraw : Trigger {
    public Card drawnCard { get; private set; }

    public OnDraw(List<TriggerAction> triggerActions) : base(TriggerType.DRAW, triggerActions) {
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