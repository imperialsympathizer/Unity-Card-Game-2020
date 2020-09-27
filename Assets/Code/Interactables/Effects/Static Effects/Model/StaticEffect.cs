public abstract class StaticEffect {
    // StaticEffects are created BY DynamicEffects and persist across a specific timeframe (a turn, multiple turns, for the rest of the game, etc.)
    // However, it is possible that a StaticEffect could, in turn, create DynamicEffects as well
    // For example, a StaticEffect could be something such as a passive of drawing an extra card every turn
    // The passive would CREATE a DrawCard DynamicEffect every turn
    // StaticEffects can also be attached to specific entities in the game (status on a character, modifier on a value)
    public int id;
    public string name;

    // There are primarily 2 different types of StaticEffects
    // Modifiers, which directly influence the value of certain parameters, such as attack on a character
    // Modifiers can only be attached to specific characters and entities, they cannot be passive
    //
    // Statuses, which tend to be more "special" effects that often require unique implementation
    // Statuses can be both attached and passive
    public enum ModifierType {
        ATK_VALUE,
        ATK_TIMES,
        MAX_LIFE,
        LIFE_VALUE,
        MAX_WILL,
        WILL_VALUE
    }

    public enum StatusType {
        INFECTION,
        UNDYING,
        DRAW_PER_TURN
    }

    public StaticEffect(int id, string name) {
        this.id = id;
        this.name = name;
    }

    // The method requiring implementation by every static effect
    // Will deal with the creation of dynamic effects, incrementing of values, removal of said static effects, etc. as needed
    // This method will be called whenever relevant (once per turn, triggered on attacks or card plays, etc.)
    public abstract void ResolveStaticEffect();
}
