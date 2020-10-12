using System;
using System.Collections.Generic;

public class IncrementValue : Modifier {
    // Generic modifier for incrementing various character values
    // effectCount: The amount to modify the value by whenever triggered (can be positive or negative)

    // This is used as the number of times to perform the effect before removing it
    // default is no limit (-1)
    private int incrementTimes;

    // Constructor with neither a stopOnValue or removeOnValue (can still be removed on a Trigger)
    public IncrementValue(
        Character character,
        ModifierType modifierType,
        int incrementValue,
        List<Trigger> triggers,
        int incrementTimes = -1) 
        : base(character, modifierType, incrementValue, triggers, 0) {
        this.incrementTimes = incrementTimes;
    }

    protected override void ResolveEffect(Trigger trigger) {
        switch (modifierType) {
            case ModifierType.ATK_VALUE:
                if (character is Fighter) {
                    ((Fighter)character).UpdateAttackValue(effectCount);
                }
                break;
            case ModifierType.ATK_TIMES:
                if (character is Fighter) {
                    ((Fighter)character).UpdateAttackTimes(effectCount);
                }
                break;
            case ModifierType.MAX_LIFE:
                if (character is Fighter) {
                    ((Fighter)character).UpdateMaxLife(effectCount);
                }
                break;
            case ModifierType.LIFE_VALUE:
                if (character is Fighter) {
                    ((Fighter)character).UpdateLifeValue(effectCount);
                }
                break;
            case ModifierType.MAX_WILL:
                if (character is Player) {
                    ((Player)character).UpdateMaxWill(effectCount);
                }
                break;
            case ModifierType.WILL_VALUE:
                if (character is Player) {
                    ((Player)character).UpdateWillValue(effectCount);
                }
                break;
        }

        if (incrementTimes > 0) {
            incrementTimes--;
            if (incrementTimes == 0) {
                // Remove the effect if this was the last time to resolve it
                RemoveEffect(trigger);
            }
        }
    }

    protected override void OperateOnEffect(Trigger trigger) {
        throw new NotImplementedException();
    }

    private void SetValue() {

    }
}
