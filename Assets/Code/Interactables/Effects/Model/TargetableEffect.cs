using System.Collections.Generic;

public abstract class TargetableEffect : PlayEffect {

    public List<Target> validTargets = new List<Target>();

    protected TargetableEffect(int repeatCount, List<Target> validTargets) : base(repeatCount) {
        this.validTargets = validTargets;
    }
    public abstract List<Target> GetValidTargets();
}