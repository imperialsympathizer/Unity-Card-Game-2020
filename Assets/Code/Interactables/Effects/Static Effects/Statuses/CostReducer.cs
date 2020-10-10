using System.Collections.Generic;

public class CostReducer : Status {
    private int reduceValue;

    public CostReducer(Character character, int reduceValue)
        : base(character,
            StatusType.COST_REDUCER,
            1,
            new List<Trigger> { // List of triggers for the effect
                new OnBeginTurn(new List<Trigger.TriggerAction> { // Subscribe to the OnBeginTurn trigger
                    Trigger.TriggerAction.RESOLVE // Execute ResolveEffect() when triggered
                })
            },
            0) {
        this.reduceValue = reduceValue;
    }

    protected override void OperateOnEffect(Trigger trigger) {}

    protected override void ResolveEffect(Trigger trigger) {
        DynamicEffectController.SharedInstance.AddEffect(new ReduceRandomCardCost(1, reduceValue));
    }
}