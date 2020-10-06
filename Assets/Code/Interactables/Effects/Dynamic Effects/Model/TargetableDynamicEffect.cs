using System;
using System.Collections.Generic;

public abstract class TargetableDynamicEffect : DynamicEffect {

    public List<Target> validTargets = new List<Target>();
    // When a card is played, the selected Targets will be added here
    public List<Tuple<int, Target>> selectedTargets = new List<Tuple<int, Target>>();

    public readonly string targetingDialogue;

    // If minTargets is 0, no targets have to be selected
    public int minTargets;
    // The highest number of selectable targets
    // if -1, there is no limit to the number of targets
    public int maxTargets;

    protected TargetableDynamicEffect(int effectCount, List<Target> validTargets, string targetingDialogue, int minTargets, int maxTargets) : base(effectCount) {
        this.validTargets = validTargets;
        this.targetingDialogue = targetingDialogue;
        this.minTargets = minTargets;
        this.maxTargets = maxTargets;
    }

    public List<Target> GetValidTargets() {
        return validTargets;
    }
}
