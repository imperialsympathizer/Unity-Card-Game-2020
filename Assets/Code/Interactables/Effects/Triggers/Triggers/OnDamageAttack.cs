using System;
using System.Collections.Generic;

[Serializable]
public class OnDamageAttack : CombatTrigger {
    public OnDamageAttack(List<TriggerAction> triggerActions) : base(triggerActions) {
        Fighter.OnDamageFromAttack += OnEventTriggered;
    }

    public override void DeactivateTrigger() {
        Fighter.OnDamageFromAttack -= OnEventTriggered;
    }

    private void OnEventTriggered(Fighter attacker, Fighter defender, int damage, int lifeResult) {
        // Data operations
        this.attacker = attacker;
        this.defender = defender;
        this.damage = damage;
        this.lifeResult = lifeResult;

        if (effect != null) {
            // Call the effect with the trigger
            effect.ResolveTrigger(this);
        }
    }
}
