public abstract class BaseEffect {
    public int id;

    // The base number of times an effect should be performed
    public int effectCount;

    public BaseEffect(int effectCount) {
        this.effectCount = effectCount;
        id = ResourceController.GenerateId();
    }
}