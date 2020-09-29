using System.Collections.Generic;

public abstract class AttachedStaticEffect : StaticEffect {
    // This StaticEffect child class deals specifically with effects that are attached to specific entities
    // If a character has a modifier or status applied to it, it will be an instance of this class

    // reference to the character that has the static effect
    protected Character character;

    public AttachedStaticEffect(Character character, int effectCount, List<Trigger> triggers, int priority) : base(effectCount, triggers, priority) {
        this.character = character;
    }

    public void AttachEffectToCharacter() {
        character.attachedEffects.Add(this.id, this);
    }

    public void RemoveEffectFromCharacter() {
        if (character.attachedEffects.ContainsKey(this.id)) {
            character.attachedEffects.Remove(this.id);
        }
    }
}
