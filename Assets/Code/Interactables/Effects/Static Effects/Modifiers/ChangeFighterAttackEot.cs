using System.Collections.Generic;

public class ChangeFighterAttackEot : Modifier {
    // Modifier for changing a fighter's attack at the end of the turn
    // effectCount: The amount to modify the value by whenever triggered (can be positive or negative)

    public ChangeFighterAttackEot(int effectCount) : base(effectCount,
        new List<Trigger> {
            new OnEndTurn(new List<Trigger.TriggerAction> { // Subscribe to the OnEndTurn trigger
                    Trigger.TriggerAction.RESOLVE // Execute ResolveEffect() when triggered
                })
        }, 0) {
    }

    protected override void ResolveEffect(Trigger trigger) {
        if (character is Fighter fighter) {
            fighter.UpdateAttackValue(effectCount);
            fighter.UpdateVisual();
        }

        // Remove the effect once resolved
        RemoveEffect(trigger);
    }

    protected override void OperateOnEffect(Trigger trigger) { }
}