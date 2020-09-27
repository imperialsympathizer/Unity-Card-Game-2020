public abstract class PassiveModifier : PassiveStaticEffect {
    // Passive modifiers will affect things across the entire battle
    // Example - all characters get +10 max life
    public ModifierType modifierType;

    // How much modifier is currently applied
    public int amount;

    // How much to change the modifier per turn
    // Can be used to make "temporary buffs" that last until EOT or statuses like DOT that decrement slightly over time
    public int incrementAmount;
}