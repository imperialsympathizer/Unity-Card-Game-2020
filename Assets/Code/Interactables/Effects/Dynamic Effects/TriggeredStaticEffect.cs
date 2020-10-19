public class TriggeredStaticEffect : DynamicEffect {
    // This is a generic DynamicEffect class for encapsulating triggered StaticEffects and working them into the DynamicEffect queue
    // When a StaticEffect is triggered, a new TriggeredStaticEffect should be instantiated and passed to the DynamicEffectController

    // The trigger object holds a reference to the effect that should be operated on
    Trigger trigger;

    public TriggeredStaticEffect(Trigger trigger) : base(1) {
        this.trigger = trigger;
    }

    public override bool IsValid() {
        return (id >= 0 && effectCount != 0 && trigger != null && trigger.effect != null);
    }

    public override void ResolveEffect() {
        trigger.effect.ResolveTrigger(trigger);

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
