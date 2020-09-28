using System.Collections.Generic;

public abstract class StaticEffect {
    // StaticEffects are created BY DynamicEffects and persist across a specific timeframe (a turn, multiple turns, for the rest of the game, etc.)
    // However, it is possible that a StaticEffect could, in turn, create DynamicEffects as well
    // For example, a StaticEffect could be something such as a passive of drawing an extra card every turn
    // The passive would create a DrawCard DynamicEffect every turn and pass it to EffectResolver
    // There are 3 different types of StaticEffects: Modifiers, Statuses, and Passives

    protected int id;

    // How many times the effect is performed when triggered
    protected int effectCount;

    // Some static effects will need to be implemented with a "timing" system of sorts to know when to trigger, that's what Triggers are for
    // Some effects have multiple Triggers, so we use a list for all Triggers that cause this effect to occur (if there are none, should pass the NONE Trigger)
    public Dictionary<TriggerAction.Trigger, TriggerAction> TriggerActions { get; private set; }

    // When multiple static effects are triggered by a Trigger, there is a priority system for the order in which effects are resolved
    // 0 is highest priority
    protected int priority;

    // All effects can be operated on (modified while the effect is "active") unless overridden by inheriting class
    bool canOperate = true;

    public StaticEffect(int effectCount, Dictionary<TriggerAction.Trigger, TriggerAction> triggerActions, int priority) {
        id = ResourceController.GenerateId();
        this.effectCount = effectCount;
        this.TriggerActions = triggerActions;
        this.priority = priority;
    }

    // The method requiring implementation by every static effect
    // Will deal with the creation of dynamic effects, incrementing of values, removal of said static effects, etc. as needed
    // This method will be called whenever relevant (once per turn, triggered on attacks or card plays, etc.)
    public void ResolveTrigger(TriggerAction.Trigger trigger) {
        if (TriggerActions.ContainsKey(trigger)) {
            List<TriggerAction.Action> actions = TriggerActions[trigger].actions;
            // Perform each actions required for the given trigger (in order)
            for (int i = 0; i < actions.Count; i++) {
                switch (actions[i]) {
                    case TriggerAction.Action.RESOLVE:
                        for (int j = 0; j < effectCount; j++) {
                            ResolveEffect();
                        }
                        break;
                    case TriggerAction.Action.REMOVE:
                        RemoveEffect();
                        break;
                    case TriggerAction.Action.OPERATE:
                        if (canOperate) {
                            OperateOnEffect();
                        }
                        break;
                    case TriggerAction.Action.STOP_OPERATE:
                        canOperate = false;
                        break;
                }
            }
        }
    }

    // Removal of StaticEffects is handled by the StaticEffectController
    // This method is overridden in Modifier, Status, and Passive abstract classes, should not be overridden by children unless special removal logic is needed
    protected abstract void RemoveEffect();

    // This is the method that will be called every time an effect gets triggered
    // If there is ever an effect that needs multiple different ResolveEffect methods, it should be split into different effect classes
    protected abstract void ResolveEffect();

    // Unless overridden by the child effect, all effects can be operated on (anything from changing relevant values to changing logic, usually related to how ResolveEffect works)
    // If an effect does not ever need to be operated on, the implementation of this method should be empty
    // Additionally, the constructor for that effect can override the canOperate flag to prevent empty method calls
    protected abstract void OperateOnEffect();
}
