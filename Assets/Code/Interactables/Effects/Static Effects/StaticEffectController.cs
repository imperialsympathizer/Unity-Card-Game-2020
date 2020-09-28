using System.Collections.Generic;

public class StaticEffectController {
    public static StaticEffectController SharedInstance;
    // This class manages static effects (both attached and passive) as the game progresses through states

    // Structure containing all attached effects that are related to characters/entities (Statuses and Modifiers)
    // Sorted by timing trigger first, then character
    // TODO: worth considering to sort by timing trigger > priority > character instead of trigger > character > priority, but that's more of a high level design decision for later
    private Dictionary<TriggerAction.Trigger, Dictionary<Character, List<AttachedStaticEffect>>> attachedEffects = new Dictionary<TriggerAction.Trigger, Dictionary<Character, List<AttachedStaticEffect>>>();

    // Structure containing all passive effects that aren't directly attached to characters/entities
    private Dictionary<TriggerAction.Trigger, List<Passive>> passiveEffects = new Dictionary<TriggerAction.Trigger, List<Passive>>();

    public void Initialize() {
        SharedInstance = this;
    }

    // Creates a new modifier on a character
    // Adds the modifier as a dictionary entry for every trigger that it has
    public void AddModifier(Modifier modifier, Character attachee) {
        foreach (KeyValuePair<TriggerAction.Trigger, TriggerAction> triggerAction in modifier.TriggerActions) {
            TriggerAction.Trigger trigger = triggerAction.Key;
            if (attachedEffects.ContainsKey(trigger)) {
                // Cache character dictionary to reduce searches
                Dictionary<Character, List<AttachedStaticEffect>> characterDictionary = attachedEffects[trigger];
                if (characterDictionary.ContainsKey(attachee)) {
                    // Updates current list of effects if there is an index for trigger and attachee already
                    List<AttachedStaticEffect> currentList = characterDictionary[attachee];
                    currentList.Add(modifier);
                    attachedEffects[trigger][attachee] = currentList;
                }
                else {
                    // Adds new list of effects if there is an index for trigger but not attachee
                    List<AttachedStaticEffect> newList = new List<AttachedStaticEffect> {
                        modifier
                    };
                    attachedEffects[trigger].Add(attachee, newList);
                }
            }
            else {
                // Adds new list of effects if there is not an index for either trigger or attachee
                List<AttachedStaticEffect> newList = new List<AttachedStaticEffect> {
                    modifier
                };
                Dictionary<Character, List<AttachedStaticEffect>> newDictionary = new Dictionary<Character, List<AttachedStaticEffect>> {
                    { attachee, newList }
                };
                attachedEffects.Add(trigger, newDictionary);
            }
        }
    }

    public void AddStatus(Status status, Character attachee) {
        foreach (KeyValuePair<TriggerAction.Trigger, TriggerAction> triggerAction in status.TriggerActions) {
            TriggerAction.Trigger trigger = triggerAction.Key;
            if (attachedEffects.ContainsKey(trigger)) {
                // Cache character dictionary to reduce searches
                Dictionary<Character, List<AttachedStaticEffect>> characterDictionary = attachedEffects[trigger];
                if (characterDictionary.ContainsKey(attachee)) {
                    // Updates current list of effects if there is an index for trigger and attachee already
                    List<AttachedStaticEffect> currentList = characterDictionary[attachee];
                    currentList.Add(status);
                    attachedEffects[trigger][attachee] = currentList;
                }
                else {
                    // Adds new list of effects if there is an index for trigger but not attachee
                    List<AttachedStaticEffect> newList = new List<AttachedStaticEffect> {
                        status
                    };
                    attachedEffects[trigger].Add(attachee, newList);
                }
            }
            else {
                // Adds new list of effects if there is not an index for either trigger or attachee
                List<AttachedStaticEffect> newList = new List<AttachedStaticEffect> {
                    status
                };
                Dictionary<Character, List<AttachedStaticEffect>> newDictionary = new Dictionary<Character, List<AttachedStaticEffect>> {
                    { attachee, newList }
                };
                attachedEffects.Add(trigger, newDictionary);
            }
        }
    }

    public void AddPassive(Passive passive) {
        foreach (KeyValuePair<TriggerAction.Trigger, TriggerAction> triggerAction in passive.TriggerActions) {
            TriggerAction.Trigger trigger = triggerAction.Key;
            if (passiveEffects.ContainsKey(trigger)) {
                // Updates current list of effects if there is an index for trigger already
                List<Passive> currentList = passiveEffects[trigger];
                currentList.Add(passive);
                passiveEffects[trigger] = currentList;
            }
            else {
                // Adds new list of effects if there is not an index for trigger
                List<Passive> newList = new List<Passive> {
                    passive
                };
                passiveEffects.Add(trigger, newList);
            }
        }
    }

    public void RemoveModifier(Modifier modifier, Character attachee) {
        foreach (KeyValuePair<TriggerAction.Trigger, TriggerAction> triggerAction in modifier.TriggerActions) {
            TriggerAction.Trigger trigger = triggerAction.Key;
            if (attachedEffects.ContainsKey(trigger) && attachedEffects[trigger].ContainsKey(attachee) && attachedEffects[trigger][attachee].Contains(modifier)) {
                attachedEffects[trigger][attachee].Remove(modifier);
            }
        }
    }

    public void RemoveStatus(Status status, Character attachee) {
        foreach (KeyValuePair<TriggerAction.Trigger, TriggerAction> triggerAction in status.TriggerActions) {
            TriggerAction.Trigger trigger = triggerAction.Key;
            if (attachedEffects.ContainsKey(trigger) && attachedEffects[trigger].ContainsKey(attachee) && attachedEffects[trigger][attachee].Contains(status)) {
                attachedEffects[trigger][attachee].Remove(status);
            }
        }
    }

    public void RemovePassive(Passive passive) {
        foreach (KeyValuePair<TriggerAction.Trigger, TriggerAction> triggerAction in passive.TriggerActions) {
            TriggerAction.Trigger trigger = triggerAction.Key;
            if (passiveEffects.ContainsKey(trigger) && passiveEffects[trigger].Contains(passive)) {
                passiveEffects[trigger].Remove(passive);
            }
        }
    }

    public List<Modifier> GetModifiersForCharacter(Modifier.ModifierType modifierType, Character character) {
        List<Modifier> returnList = new List<Modifier>();
        foreach (KeyValuePair <TriggerAction.Trigger, Dictionary<Character, List<AttachedStaticEffect>>> characterDictionary in attachedEffects) {
            // If the trigger has any effects for the character that match the modifier type, add them to the return list
            if (characterDictionary.Value.ContainsKey(character)) {
                List<AttachedStaticEffect> attachedList = characterDictionary.Value[character];
                for (int i = 0; i < attachedList.Count; i++) {
                    AttachedStaticEffect effect = attachedList[i];
                    if (effect is Modifier modifier) {
                        if (modifier.modifierType == modifierType) {
                            returnList.Add(modifier);
                        }
                    }
                }
            }
        }

        return returnList;
    }
}
