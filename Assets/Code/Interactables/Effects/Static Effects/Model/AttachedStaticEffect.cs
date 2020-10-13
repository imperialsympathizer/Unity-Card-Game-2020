using System.Collections.Generic;

public abstract class AttachedStaticEffect : StaticEffect {
    // This StaticEffect child class deals specifically with effects that are attached to specific entities
    // If a character has a modifier or status applied to it, it will be an instance of this class

    // reference to the character that has the static effect
    protected Character character;

    public bool IsAttached { get; private set; }

    public AttachedStaticEffect(int effectCount, List<Trigger> triggers, int priority) : base(effectCount, triggers, priority) {
        IsAttached = false;
    }

    public void AttachEffectToCharacter(Character character) {
        if (!IsAttached) {
            this.character = character;
            this.character.attachedEffects.Add(this.id, this);
            ActivateTriggers();
            IsAttached = true;
        }
    }

    public void RemoveEffectFromCharacter() {
        if (IsAttached && character.attachedEffects.ContainsKey(this.id)) {
            character.attachedEffects.Remove(this.id);
        }
        IsAttached = false;
    }
}
