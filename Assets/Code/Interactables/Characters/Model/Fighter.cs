public abstract class Fighter : Attacker {
    // abstract class defining a combatant that has both health and attack

    public Fighter(string name, int baseAttack, int baseAttackTimes, bool hasLife = false, int baseMaxLife = 0, int baseLife = 0) : base(name, baseAttack, baseAttackTimes) {
        HasLife = hasLife;
        MaxLife = baseMaxLife;
        LifeValue = baseLife;
    }
    
    public bool HasLife { get; private set; }
    public int MaxLife { get; private set; }
    public int LifeValue { get; private set; }

    public void UpdateMaxLife(int valueChange) {
        MaxLife += valueChange;
    }

    public void UpdateLifeValue(int valueChange) {
        LifeValue += valueChange;
        if (LifeValue > MaxLife) {
            // Life value cannot exceed max life
            LifeValue = MaxLife;
        }
    }
}
