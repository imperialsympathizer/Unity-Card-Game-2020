using System.Collections.Generic;

public class Infected : Status {
    // This status goes on characters that are infected
    // At the beginning of each turn, the character takes damage equal to infectionValue, then infectionValue is decremented by 1
    // if infectionValue reaches zero, remove this effect

    // The amount of infection that is applied to a character on a specific trigger (can be any character, not necessarily the character with the Infector status)
    public int infectionValue;

    public Infected(Character character, int infectionValue) :
        base(
        character,
        StatusType.INFECTOR,
        1,
        new List<Trigger> { // List of triggers for the effect
            new OnBeginTurn(new List<Trigger.TriggerAction> { // Subscribe to the BeginTurn trigger
                Trigger.TriggerAction.RESOLVE // Execute ResolveEffect() when triggered
            })
        },
        0) {
        this.infectionValue = infectionValue;
    }

    protected override void ResolveEffect(Trigger trigger) {
        if (character is Fighter fighter && fighter.HasLife) {
            // To prevent infinite loops, do not trigger other effeects
            // TODO: consider allowing infinite loops, or looping for a prescribed amount
            fighter.UpdateLifeValue(-infectionValue, false);
            infectionValue--;
            if (infectionValue == 0) {
                RemoveEffect(trigger);
            }
        }
    }

    protected override void OperateOnEffect(Trigger trigger) {
    }
}
