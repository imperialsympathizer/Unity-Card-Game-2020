using System.Collections.Generic;

public abstract class Trigger {
    // This is the parent class for triggers in the game
    // Since each trigger may have unique information that needs to be passed to the StaticEffect, they are implemented as separate classes

    // Every trigger has a type which indicates the event that it is listening too
    // public readonly TriggerType triggerType;

    // Reference to the effect to call when triggered
    protected StaticEffect effect;

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


    // Events that will cause effects to do things
    // Each event will have its own Trigger class that subscribes to the event for a specified effect
    //public enum TriggerType {
    //    BEGIN_BATTLE,
    //    END_BATTLE,
    //    BEGIN_TURN,
    //    END_TURN,
    //    BEGIN_COMBAT,
    //    END_COMBAT,
    //    ATTACK,         // When attacking
    //    DAMAGE_ATK,     // When damaged by an attack
    //    DAMAGE_NO_ATK,  // When damaged by a non-attack
    //    HEAL,
    //    DEATH,
    //    DRAW,
    //    DISCARD,
    //    CARD_PLAY
    //}

    // What the effect should do on a specific trigger
    public enum TriggerAction {
        RESOLVE,        // Resolve the effect
        REMOVE,         // Remove the effect
        OPERATE,        // Operate on the effect (change values, etc.)
        STOP_OPERATE,   // Stop future operations from occurring on the effect
    }
}