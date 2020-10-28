using System;

public abstract class Fighter : Character {
    // abstract class defining a combatant that has both health and attack
    public readonly FighterType fighterType;

    public int AttackValue { get { return attackValue; } }
    private int attackValue;
    public int AttackTimes { get { return attackTimes; } }
    private int attackTimes;
    public bool HasLife { get { return hasLife; } }
    private bool hasLife;
    public int MaxLife { get { return maxLife; } }
    private int maxLife;
    public int LifeValue { get { return lifeValue; } }
    private int lifeValue;

    public enum FighterType {
        PLAYER,
        SUMMON,
        ENEMY
    }

    public static event Action<Fighter, Fighter> OnAttack;
    public static event Action<Fighter, Fighter, int, int> OnDamageFromAttack;
    public static event Action<Fighter, int, int> OnDamageFromNonAttack;
    public static event Action<Fighter, int, int> OnHeal;
    public static event Action<int, int, int> OnAttackChange;

    public Fighter(string name, FighterType fighterType, int baseAttack, int baseAttackTimes, bool hasLife = false, int baseMaxLife = 0, int baseLife = 0) : base(name, "") {
        this.fighterType = fighterType;
        attackValue = baseAttack;
        attackTimes = baseAttackTimes;
        this.hasLife = hasLife;
        maxLife = baseMaxLife;
        lifeValue = baseLife;
    }

    public void UpdateAttackValue(int valueChange) {
        // Add or subtract an value from the current
        attackValue += valueChange;
        OnAttackChange?.Invoke(Id, attackValue, attackTimes);
    }

    public void UpdateAttackTimes(int valueChange) {
        attackTimes += valueChange;
        OnAttackChange?.Invoke(Id, attackValue, attackTimes);
    }

    public bool UpdateMaxLife(int valueChange) {
        maxLife += valueChange;
        if (lifeValue > maxLife) {
            return UpdateLifeValue(0, false);
        }
        else {
            UpdateVisual();
            return CheckDeath();
        }
    }

    // Updates life value and returns if the character is considered dead
    public bool UpdateLifeValue(int valueChange, bool triggerEvents = true) {
        lifeValue += valueChange;
        if (lifeValue > maxLife) {
            // Life value cannot exceed max life
            lifeValue = maxLife;
        }
        if (valueChange > 0 && triggerEvents) {
            OnHeal?.Invoke(this, valueChange, lifeValue);
        }
        else if (valueChange < 0 && triggerEvents) {
            OnDamageFromNonAttack?.Invoke(this, valueChange, lifeValue);
        }
        UpdateVisual();
        return CheckDeath();
    }

    public bool CheckDeath() {
        if (hasLife && (lifeValue < 1 || maxLife < 1)) {
            return true;
        }
        return false;
    }

    public void ReceiveAttack(Fighter attacker) {
        // Invoke the OnAttack event before dealing damage
        // Allows for buffs on attack triggers before damage is dealt
        OnAttack?.Invoke(attacker, this);
        UpdateLifeValue(-attacker.AttackValue, false);
        // Invoke damage from attack event
        OnDamageFromAttack?.Invoke(attacker, this, attacker.AttackValue, lifeValue);
    }

    public abstract void PerformAttack();

    protected void InvokeOnAttack(Fighter attacker, Fighter defender) {
        OnAttack?.Invoke(attacker, defender);
    }

    protected void InvokeOnDamageFromAttack(Fighter attacker, Fighter defender, int attackValue, int resultingLife) {
        OnDamageFromAttack?.Invoke(attacker, defender, attackValue, resultingLife);
    }
}
