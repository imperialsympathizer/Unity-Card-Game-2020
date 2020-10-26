using System;
using System.Collections.Generic;

[Serializable]
public abstract class TurnTrigger : Trigger {
    public int turnNumber { get; protected set; }

    public TurnTrigger(List<TriggerAction> triggerActions) : base(triggerActions) {
    }
}