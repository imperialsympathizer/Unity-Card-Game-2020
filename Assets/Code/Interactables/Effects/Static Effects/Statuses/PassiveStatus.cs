public abstract class PassiveStatus : PassiveStaticEffect {
    // Because of how broadly applicable this class is, the brunt of the logic will be left to the instantiable child classes
    // This class could cover anything from drawing an extra card per turn to making cards cost less to skipping the next combat step, etc.

    public StatusType statusType;

    public PassiveStatus(int id, StatusType statusType, string name) : base(id, name) {
        this.statusType = statusType;
    }
}
