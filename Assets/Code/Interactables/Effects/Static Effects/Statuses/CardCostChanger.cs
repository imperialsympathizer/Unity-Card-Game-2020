using System.Collections.Generic;

public class CardCostChanger : Status {
    public CardCostChanger(Character character, int changeValue)
        : base(character,
            StatusType.COST_REDUCER,
            changeValue,
            new List<Trigger> { // List of triggers for the effect
                new OnBeginTurn(new List<Trigger.TriggerAction> { // Subscribe to the OnBeginTurn trigger
                    Trigger.TriggerAction.RESOLVE // Execute ResolveEffect() when triggered
                })
            },
            0) {}

    protected override void OperateOnEffect(Trigger trigger) {}

    protected override void ResolveEffect(Trigger trigger) {
        DynamicEffectController.SharedInstance.AddEffect(new ChangeRandomCardCost(effectCount));
    }
}