using System.Collections.Generic;

public abstract class Character {
    // highest level of abstraction for characters (summons, enemies, players)
    // No instantiable classes should be directly inheriting from this class
    public readonly int id;
    public readonly string name;

    // List of modifiers and statuses attached to a character
    // Characters will always have no attachedEffects when initialized, but can have them added immediately after initialization
    // This can be useful for "spawning" characters that have certain characteristics
    // This list also manages effects that get attached to the character through outside effects
    // An example of this is a character may become infected, so a new Infected status would be added to this list
    public Dictionary<int, AttachedStaticEffect> attachedEffects = new Dictionary<int, AttachedStaticEffect>();

    public Character(string name) {
        this.id = ResourceController.GenerateId();
        this.name = name;
    }
}
