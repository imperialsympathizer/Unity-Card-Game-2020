using System.Collections.Generic;

public class Infected : Status {
    // This status goes on characters that are infected
    // At the beginning of each turn, the character takes damage equal to effectCount, then effectCount is decremented by 1
    // if effectCount reaches zero, remove this effect

    public Infected(Character character, int infectionValue) :
        base(
        character,
        StatusType.INFECTOR,
        infectionValue,
        new List<Trigger> { // List of triggers for the effect
            new OnBeginTurn(new List<Trigger.TriggerAction> { // Subscribe to the BeginTurn trigger
                Trigger.TriggerAction.RESOLVE // Execute ResolveEffect() when triggered
            })
        },
        0) {}

    protected override void ResolveEffect(Trigger trigger) {
        if (character is Fighter fighter && fighter.HasLife) {
            // To prevent infinite loops, do not trigger other effeects
            // TODO: consider allowing infinite loops, or looping for a prescribed amount
            fighter.UpdateLifeValue(-effectCount, false);
            effectCount--;
            if (effectCount == 0) {
                RemoveEffect(trigger);
            }
        }
    }

    protected override void OperateOnEffect(Trigger trigger) {
    }
}
