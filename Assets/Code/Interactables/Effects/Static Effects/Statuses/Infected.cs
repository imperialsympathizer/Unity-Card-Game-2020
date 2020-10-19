using System.Collections.Generic;

public class Infected : Status {
    // This status goes on characters that are infected
    // At the beginning of each turn, the character takes damage equal to effectCount, then effectCount is decremented by 1
    // if effectCount reaches zero, remove this effect

    public Infected(int effectCount) :
        base(
        effectCount,
        new List<Trigger> { // List of triggers for the effect
            new OnBeginTurn(new List<Trigger.TriggerAction> { // Subscribe to the BeginTurn trigger
                Trigger.TriggerAction.RESOLVE // Execute ResolveEffect() when triggered
            })
        },
        0) { }

    protected override void ResolveEffect(Trigger trigger) {
        if (character is Fighter fighter && fighter.HasLife) {
            // TODO: consider preventing infinite loops, or looping for a prescribed amount
            DynamicEffectController.SharedInstance.AddEffect(new ChangeFighterHealth(-effectCount, fighter));
            effectCount--;
            if (effectCount == 0) {
                RemoveEffect(trigger);
            }
        }
    }

    protected override void OperateOnEffect(Trigger trigger) {
    }
}
