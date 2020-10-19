using System;

public abstract class BaseEffect {
    public int id;

    // The base number of times an effect should be performed
    public int effectCount;

    // This event should be fired when updating effectCount value (int is value change)
    public static event Action<int> OnEffectCountChange;

    public BaseEffect(int effectCount) {
        this.effectCount = effectCount;
        id = ResourceController.GenerateId();
    }

    // Adds a value to effectCount (can be positive or negative)
    public void UpdateEffectCount(int valueChange) {
        effectCount += valueChange;
        if (valueChange != 0) {
            OnEffectCountChange?.Invoke(valueChange);
        }
    }
}