using System;

public abstract class Fighter : Character {
    // abstract class defining a combatant that has both health and attack
    public readonly FighterType fighterType;

    public int AttackValue { get; private set; }
    public int AttackTimes { get; private set; }
    public bool HasLife { get; private set; }
    public int MaxLife { get; private set; }
    public int LifeValue { get; private set; }

    public static Action<Fighter, Fighter> OnAttack;
    public static Action<Fighter, Fighter, int, int> OnDamageAttack;
    public static Action<Fighter, int, int> OnDamageNonAttack;
    public static Action<Fighter, int, int> OnHeal;

    public Fighter(string name, FighterType fighterType, int baseAttack, int baseAttackTimes, bool hasLife = false, int baseMaxLife = 0, int baseLife = 0) : base(name) {
        this.fighterType = fighterType;
        AttackValue = baseAttack;
        AttackTimes = baseAttackTimes;
        HasLife = hasLife;
        MaxLife = baseMaxLife;
        LifeValue = baseLife;
    }

    public void UpdateAttackValue(int valueChange) {
        // Add or subtract an value from the current
        AttackValue += valueChange;
    }

    public void UpdateAttackTimes(int valueChange) {
        AttackTimes += valueChange;
    }

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

    public void ReceiveAttack(Fighter attacker) {
        // Invoke the OnAttack event before dealing damage
        // Allows for buffs on attack triggers before damage is dealt
        OnAttack?.Invoke(attacker, this);
        LifeValue -= attacker.AttackValue;
        // Invoke damage from attack event
        OnDamageAttack?.Invoke(attacker, this, attacker.AttackValue, LifeValue);
    }

    public abstract void PerformAttack();

    public enum FighterType {
        PLAYER,
        SUMMON,
        ENEMY
    }
}
