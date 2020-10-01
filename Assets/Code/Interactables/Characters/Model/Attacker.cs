using System;

public abstract class Attacker : Character {
    // abstract class defining an attacking character, which generally is all characters
    // Some attackers may not have health, so Life and Attack fields are split into separate abstract classes
    // Example - Spirits can attack but do not have health
    // Attackers should be used instead of Characters even if a character is not designed to attack - it should just have baseAttack = 0 and/or baseAttackTimes = 0
    public Attacker(string name, int baseAttack, int baseAttackTimes) : base(name) {
        AttackValue = baseAttack;
        AttackTimes = baseAttackTimes;
    }
    
    public int AttackValue { get; private set; }
    public int AttackTimes { get; private set; }

    public static Action<Attacker, Fighter> OnAttack;

    public void UpdateAttackValue(int valueChange) {
        // Add or subtract an value from the current
        AttackValue += valueChange;
    }

    public void UpdateAttackTimes(int valueChange) {
        AttackTimes += valueChange;
    }

    public abstract void PerformAttack();
}
