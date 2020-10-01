using System;

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

    public static Action<Attacker, Fighter, int, int> OnDamageAttack;
    public static Action<Fighter, int, int> OnDamageNonAttack;
    public static Action<Fighter, int, int> OnHeal;

    public void UpdateMaxLife(int valueChange) {
        MaxLife += valueChange;
        UpdateVisual();
        TurnSystem.SharedInstance.CheckGameConditions();
    }

    public void UpdateLifeValue(int valueChange, bool triggerEvents = true) {
        LifeValue += valueChange;
        if (LifeValue > MaxLife) {
            // Life value cannot exceed max life
            LifeValue = MaxLife;
        }
        if (valueChange > 0 && triggerEvents) {
            OnHeal?.Invoke(this, valueChange, LifeValue);
        }
        else if (valueChange < 0 && triggerEvents) {
            OnDamageNonAttack?.Invoke(this, valueChange, LifeValue);
        }
        UpdateVisual();
        TurnSystem.SharedInstance.CheckGameConditions();
    }

    public void ReceiveAttack(Attacker attacker) {
        // Invoke the OnAttack event before dealing damage
        // Allows for buffs on attack triggers before damage is dealt
        OnAttack?.Invoke(attacker, this);
        LifeValue -= attacker.AttackValue;
        // Invoke damage from attack event
        OnDamageAttack?.Invoke(attacker, this, attacker.AttackValue, LifeValue);
    }
}
