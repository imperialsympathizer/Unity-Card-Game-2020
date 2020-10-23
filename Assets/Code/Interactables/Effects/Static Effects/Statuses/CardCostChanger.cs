using System.Collections.Generic;

public class CardCostChanger : Status {
    public CardCostChanger(int effectCount)
        : base(
            effectCount,
            new List<Trigger> { // List of triggers for the effect
                new OnBeginTurn(new List<Trigger.TriggerAction> { // Subscribe to the OnBeginTurn trigger
                    Trigger.TriggerAction.RESOLVE // Execute ResolveEffect() when triggered
                })
            },
            0) { }

    protected override void OperateOnEffect(Trigger trigger) { }

    protected override void ResolveEffect(Trigger trigger) {
        DynamicEffectController.Instance.AddEffect(new ChangeRandomCardCost(effectCount));
    }
}