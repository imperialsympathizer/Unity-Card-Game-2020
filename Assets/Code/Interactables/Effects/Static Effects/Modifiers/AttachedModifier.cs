public abstract class AttachedModifier : AttachedStaticEffect {
    public ModifierType modifierType;

    // How much modifier is currently applied
    public int amount;

    // How much to change the modifier per turn
    // Can be used to make "temporary buffs" that last until EOT or statuses like DOT that decrement slightly over time
    public int incrementAmount;
}