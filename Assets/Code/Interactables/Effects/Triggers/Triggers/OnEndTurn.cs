using System;
using System.Collections.Generic;

[Serializable]
public class OnEndTurn : TurnTrigger {

    public OnEndTurn(List<TriggerAction> triggerActions) : base(triggerActions) {
        EndTurn.OnEndTurn += OnEventTriggered;
    }

    public override void DeactivateTrigger() {
        EndTurn.OnEndTurn -= OnEventTriggered;
    }

    private void OnEventTriggered(int turnNumber) {
        // Data operations
        this.turnNumber = turnNumber;

        if (effect != null) {
            // Call the effect with the trigger
            effect.ResolveTrigger(this);
        }
    }
}
