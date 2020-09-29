using System.Collections.Generic;

public abstract class TurnTrigger : Trigger {
    public int turnNumber { get; protected set; }

    public TurnTrigger(TriggerType triggerType, List<TriggerAction> triggerActions) : base(triggerType, triggerActions) {
    }
}