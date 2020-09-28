using System.Collections.Generic;

public abstract class AttachedStaticEffect : StaticEffect {
    // This StaticEffect child class deals specifically with effects that are attached to specific entities
    // If a character has a modifier or status applied to it, it will be an instance of this class

    // reference to the character that has the static effect
    protected Character character;

    public AttachedStaticEffect(Character character, int effectCount, Dictionary<TriggerAction.Trigger, TriggerAction> triggerActions, int priority) : base(effectCount, triggerActions, priority) {
        this.character = character;
    }
}
