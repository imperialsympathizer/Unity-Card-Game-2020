using System.Collections.Generic;

public class OnBeginTurn : TurnTrigger {

    public OnBeginTurn(List<TriggerAction> triggerActions) : base(triggerActions) {
        BeginTurn.OnBeginTurn += OnEventTriggered;
    }

    public override void DeactivateTrigger() {
        BeginTurn.OnBeginTurn -= OnEventTriggered;
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
