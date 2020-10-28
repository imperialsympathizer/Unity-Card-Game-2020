using System;
using System.Collections.Generic;

[Serializable]
public abstract class Trigger {
    // This is the parent class for triggers in the game
    // Since each trigger may have unique information that needs to be passed to the StaticEffect, they are implemented as separate classes

    // Reference to the effect to call when triggered
    public StaticEffect effect;

    // What actions to take on the specified trigger
    public readonly List<TriggerAction> triggerActions;

    public Trigger(List<TriggerAction> triggerActions) {
        this.triggerActions = triggerActions;
    }

    // All Triggers should have their own OnEventTriggered method that passes the data from the event through to the effect
    // Example:
    // protected override void OnEventTriggered(<data from event>) {
    //     ... Data operations
    //
    //     effect.ResolveTrigger(this);
    // }

    // When an effect is removed, it should iterate through the triggers it uses and unsubscribe from the events it was subscribed to
    public void InitializeTrigger(StaticEffect effect) {
        this.effect = effect;
    }

    public abstract void DeactivateTrigger();

    // What the effect should do on a specific trigger
    public enum TriggerAction {
        RESOLVE,        // Resolve the effect
        REMOVE,         // Remove the effect
        OPERATE,        // Operate on the effect (change values, etc.)
        STOP_OPERATE,   // Stop future operations from occurring on the effect
    }
}