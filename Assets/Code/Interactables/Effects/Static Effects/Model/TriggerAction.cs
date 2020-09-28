using System.Collections.Generic;

public class TriggerAction {
    // class that pairs Trigger enums with a list of actions (ordered) to perform on trigger

    // What trigger to listen for
    public readonly Trigger trigger;

    // What actions to take on the specified trigger
    public readonly List<Action> actions;

    public TriggerAction(Trigger trigger, List<Action> actions) {
        this.trigger = trigger;
        this.actions = actions;
    }

    // Enum for the triggers that will cause effects to do something
    public enum Trigger {
        NONE,           // Some static effects will never be triggered, they are simply persistant
        BEGIN_TURN,
        END_TURN,
        BEGIN_COMBAT,
        END_COMBAT,
        ATTACK,         // When attacking
        HIT,            // When hit by an attack
        DAMAGE,         // When damaged, should not be used in conjunction with HIT
        DAMAGE_NO_ATK,  // When damaged by a non-attack
        HEAL,
        DEATH,
        DRAW,
        DISCARD,
        CARD_PLAY
    }

    // What the effect should do on a specific trigger
    public enum Action {
        RESOLVE,        // Resolve the effect
        REMOVE,         // Remove the effect
        OPERATE,        // Operate on the effect (change values, etc.)
        STOP_OPERATE,   // Stop future operations from occurring on the effect
    }
}
