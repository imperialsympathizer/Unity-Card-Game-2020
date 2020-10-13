using System.Collections.Generic;

public abstract class CombatTrigger : Trigger {
    public Fighter attacker { get; protected set; }
    public Fighter defender { get; protected set; }
    public int damage { get; protected set; }
    public int lifeResult { get; protected set; }

    public CombatTrigger(List<TriggerAction> triggerActions) : base(triggerActions) {
    }
}