public abstract class AttachedStatus : AttachedStaticEffect {
    public StatusType statusType;

    // reference to the object that has the status
    public Fighter statusable;

    // How much status is currently applied
    public int amount;

    // How much status to lose per turn
    // Can be used to make "temporary buffs" that last until EOT or statuses like DOT that decrement slightly over time
    public int decrementAmount;
}