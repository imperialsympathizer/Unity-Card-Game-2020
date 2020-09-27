using System.Collections.Generic;

public abstract class TargetablePlayEffect : PlayEffect {

    public List<Target> validTargets = new List<Target>();

    protected TargetablePlayEffect(int repeatCount, List<Target> validTargets) : base(repeatCount) {
        this.validTargets = validTargets;
    }
    public abstract List<Target> GetValidTargets();
}
