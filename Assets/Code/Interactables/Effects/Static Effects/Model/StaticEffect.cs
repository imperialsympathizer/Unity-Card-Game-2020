using System.Collections.Generic;

public abstract class StaticEffect {
    // StaticEffects are created BY DynamicEffects and persist across a specific timeframe (a turn, multiple turns, for the rest of the game, etc.)
    // However, it is possible that a StaticEffect could, in turn, create DynamicEffects as well
    // For example, a StaticEffect could be something such as a passive of drawing an extra card every turn
    // The passive would create a DrawCard DynamicEffect every turn and pass it to EffectResolver
    // StaticEffects can also be attached to specific entities in the game (status on a character, modifier on a value)

    // There are 3 different types of StaticEffects
    // Modifiers, which directly influence the value of certain parameters, such as attack on a character
    // Modifiers can only be attached to specific characters and entities, they cannot be passive
    //
    // Statuses, which tend to be more "special" effects that often require unique implementation
    // Statuses can only be attached to specific characters and entities, they cannot be passive
    //
    // Both Status and Modifier inherit from AttachedStaticEffect (a child of StaticEffect) due to their restrictions on placements
    //
    // Passives are the last StaticEffect, and basically encompass any effect that does not require a specific object to be tied to
    // Passives can cover anything from drawing extra cards per turn to skipping combat steps, etc.

    protected int id;

    // How many times the effect is performed when triggered
    protected int effectCount;

    // Some static effects will need to be implemented with a "timing" system of sorts to know when to trigger, that's what Triggers are for
    // Some effects have multiple Triggers, so we use a list for all Triggers that cause this effect to occur (if there are none, should pass the NONE Trigger)
    public List<EffectTiming.Trigger> timingTriggers { get; private set; }

    // When multiple static effects are triggered by a Trigger, there is a priority system for the order in which effects are resolved
    // 0 is highest priority
    protected int priority;

    public StaticEffect(int effectCount, List<EffectTiming.Trigger> timingTriggers, int priority) {
        id = ResourceController.GenerateId();
        this.effectCount = effectCount;
        this.timingTriggers = timingTriggers;
        this.priority = priority;
    }

    // The method requiring implementation by every static effect
    // Will deal with the creation of dynamic effects, incrementing of values, removal of said static effects, etc. as needed
    // This method will be called whenever relevant (once per turn, triggered on attacks or card plays, etc.)
    public abstract void ResolveStaticEffect();
}
