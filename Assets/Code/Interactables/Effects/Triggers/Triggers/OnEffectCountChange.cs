using System.Collections.Generic;

public class OnEffectCountChange : Trigger {
    int valueChange;

    public OnEffectCountChange(List<TriggerAction> triggerActions) : base(triggerActions) {
        BaseEffect.OnEffectCountChange += OnEventTriggered;
    }

    public override void DeactivateTrigger() {
        BaseEffect.OnEffectCountChange -= OnEventTriggered;
    }

    private void OnEventTriggered(int valueChange) {
        // Data operations
        this.valueChange = valueChange;

        if (effect != null) {
            // Add the trigger/effect pair to the DynamicEffectController queue for resolution in FIFO order
            DynamicEffectController.SharedInstance.AddEffect(new TriggeredStaticEffect(this));
        }
    }
}