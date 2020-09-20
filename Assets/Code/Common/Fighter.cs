using System;

public class Fighter {
    // abstract class defining any combatant

    // Some fighters may not be hittable (spirits?)
    public bool HasLife { get; set; }
    public int MaxLife { get; set; }
    public int LifeValue { get; set; }

    // All fighters have attack and attackTimes (even if they are zero)
    public int AttackValue { get; set; }
    public int AttackTimes { get; set; }
}