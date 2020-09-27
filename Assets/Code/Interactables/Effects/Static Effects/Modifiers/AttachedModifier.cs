public abstract class AttachedModifier : AttachedStaticEffect {
    public ModifierType modifierType;

    // How much modifier is currently applied
    public int amount;

    // How much to change the modifier per turn (can be positive or negative)
    // Can be used to make "temporary buffs" that last until EOT or statuses like DOT that decrement slightly over time
    public int incrementAmount;

    // This boolean controls whether a modifier should be removed if the amount ever falls below zero
    // This can be useful for temporary modifiers that decrement by an amount every turn and should be removed after the amount reaches zero
    // Otherwise, amount would become negative and the character would get persisting negative effects
    // Obviously sometimes it would be useful to have a decrementing amount persist, but this is the control field for that
    public bool removeOnZero;
    public AttachedModifier(Fighter character, int id, ModifierType modifierType, int amount, int incrementAmount, bool removeOnZero) : base(character, id, modifierType.ToString()) {
        this.modifierType = modifierType;
        this.amount = amount;
        this.incrementAmount = incrementAmount;
        this.removeOnZero = removeOnZero;
    }
}