using System.Collections.Generic;

public abstract class TargetableDynamicEffect : DynamicEffect {

    public List<Target> validTargets = new List<Target>();

    protected TargetableDynamicEffect(int effectCount, List<Target> validTargets) : base(effectCount) {
        this.validTargets = validTargets;
    }
    public abstract List<Target> GetValidTargets();
}
