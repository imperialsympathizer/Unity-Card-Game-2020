using System.Collections.Generic;

public abstract class Fighter {
    // abstract class defining any combatant

    // Some fighters may not be hittable (spirits?)
    public bool HasLife { get; set; }
    public int MaxLife { get; set; }
    public int LifeValue { get; set; }

    // All fighters have attack and attackTimes (even if they are zero)
    public int AttackValue { get; set; }
    public int AttackTimes { get; set; }

    // Deals with any statuses that exist for any amount of time (e.g. not an instantaneous, 1-time effect)
    // Key - status type
    // Value - list of modifiers for a specific StatusType
    public Dictionary<StaticEffect.StatusType, List<AttachedStatus>> statuses;

    // Deals with any modifiers that exist for any amount of time (e.g. not an instantaneous, 1-time effect)
    // Key - modifier type
    // Value - list of modifiers for a specific ModifierType
    public Dictionary<StaticEffect.ModifierType, List<AttachedModifier>> modifiers;

    public void AddModifier(AttachedModifier modifier) {
        // Do not overwrite existing modifiers with new modifiers, even if there are multiple modifiers of the same type
        // This is to prevent "permanent" modifiers from overlapping with "temporary" modifiers
        // For example, if there is a modifier for +10 health until the end of the turn, the amount would be 10 and incrementAmount = -10
        // Once the value goes to 0 from the increment, the modifier will be removed
        // If there were other health modifiers that were permanent though, the incrementAmount from the temporary modifier would not interact properly

        // Current modifiers that are the same type as the modifier to add
        List<AttachedModifier> currentModifiers;
        if (modifiers.TryGetValue(modifier.modifierType, out currentModifiers)) {
            // if a list already exists for the given ModifierType, just add the new modifier to the list
            currentModifiers.Add(modifier);
            modifiers[modifier.modifierType] = currentModifiers;
        }
        else {
            // Otherwise, add a new List to the Dictionary for that ModifierType
            List<AttachedModifier> newModifierList = new List<AttachedModifier>();
            newModifierList.Add(modifier);
            modifiers.Add(modifier.modifierType, newModifierList);
        }
    }

    public void AddStatus(AttachedStatus status) {
        // Behaves similarly to AddModifier
        List<AttachedStatus> currentStatuses;
        if (statuses.TryGetValue(status.statusType, out currentStatuses)) {
            currentStatuses.Add(status);
            statuses[status.statusType] = currentStatuses;
        }
        else {
            List<AttachedStatus> newStatusList = new List<AttachedStatus>();
            newStatusList.Add(status);
            statuses.Add(status.statusType, newStatusList);
        }
    }

    public void ResolveStaticEffects() {

    }
}