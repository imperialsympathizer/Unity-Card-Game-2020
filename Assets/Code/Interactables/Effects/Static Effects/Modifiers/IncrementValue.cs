using System;
using System.Collections.Generic;

public class IncrementValue : Modifier {
    // Generic modifier for incrementing various character values

    // The amount to modify the value by whenever triggered (can be positive or negative)
    private int incrementValue;

    // This is used as the number of times to perform the effect before removing it
    // default is no limit (-1)
    private int incrementTimes;

    // Constructor with neither a stopOnValue or removeOnValue (can still be removed on a Trigger)
    public IncrementValue(
        Character character,
        ModifierType modifierType,
        int effectCount,
        Dictionary<TriggerAction.Trigger, TriggerAction> triggerActions,
        int incrementValue,
        int incrementTimes = -1) 
        : base(character, modifierType, effectCount, triggerActions, 0) {
        this.incrementValue = incrementValue;
        this.incrementTimes = incrementTimes;
    }

    protected override void ResolveEffect() {
        switch (modifierType) {
            case ModifierType.ATK_VALUE:
                if (character is Attacker) {
                    ((Attacker)character).UpdateAttackValue(incrementValue);
                }
                break;
            case ModifierType.ATK_TIMES:
                if (character is Attacker) {
                    ((Attacker)character).UpdateAttackTimes(incrementValue);
                }
                break;
            case ModifierType.MAX_LIFE:
                if (character is Fighter) {
                    ((Fighter)character).UpdateMaxLife(incrementValue);
                }
                break;
            case ModifierType.LIFE_VALUE:
                if (character is Fighter) {
                    ((Fighter)character).UpdateLifeValue(incrementValue);
                }
                break;
            case ModifierType.MAX_WILL:
                if (character is Player) {
                    ((Player)character).UpdateMaxWill(incrementValue);
                }
                break;
            case ModifierType.WILL_VALUE:
                if (character is Player) {
                    ((Player)character).UpdateWillValue(incrementValue);
                }
                break;
        }

        if (incrementTimes > 0) {
            incrementTimes--;
            if (incrementTimes == 0) {
                // Remove the effect if this was the last time to resolve it
                RemoveEffect();
            }
        }
    }

    protected override void OperateOnEffect() {
        throw new NotImplementedException();
    }

    private void SetValue() {

    }
}
