using System.Collections.Generic;

public abstract class TargetablePlayEffect : PlayEffect {

    public List<Target> validTargets = new List<Target>();

    protected TargetablePlayEffect(int effectCount, List<Target> validTargets) : base(effectCount) {
        this.validTargets = validTargets;
    }
    public abstract List<Target> GetValidTargets();
}
