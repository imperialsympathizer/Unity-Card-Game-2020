using System;
using System.Collections.Generic;

[Serializable]
public class IncrementMaxWill : Modifier {
    // Modifier for incrementing character max will
    // effectCount: The amount to modify the value by whenever triggered (can be positive or negative)

    // This is used as the number of times to perform the effect before removing it
    // default is no limit (-1)
    private int incrementTimes;

    public IncrementMaxWill(int effectCount, List<Trigger> triggers, int incrementTimes = -1) : base(effectCount, triggers, 0) {
        this.incrementTimes = incrementTimes;
    }

    protected override void ResolveEffect(Trigger trigger) {
        if (character is Player player) {
            player.UpdateMaxWill(effectCount);
        }

        if (incrementTimes > 0) {
            incrementTimes--;
            if (incrementTimes == 0) {
                // Remove the effect if this was the last time to resolve it
                RemoveEffect(trigger);
            }
        }
    }

    protected override void OperateOnEffect(Trigger trigger) { }
}