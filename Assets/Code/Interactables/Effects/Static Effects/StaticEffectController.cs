using System.Collections.Generic;
using System.Net.Mail;

public class StaticEffectController {
    public static StaticEffectController SharedInstance;
    // This class manages static effects (both attached and passive) as the game progresses through states

    // Structure containing all attached effects that are related to characters/entities (Statuses and Modifiers)
    // Sorted by timing trigger first, then character
    // TODO: worth considering to sort by timing trigger > priority > character instead of trigger > character > priority, but that's more of a high level design decision for later
    Dictionary<EffectTiming.Trigger, Dictionary<Character, List<AttachedStaticEffect>>> attachedEffects = new Dictionary<EffectTiming.Trigger, Dictionary<Character, List<AttachedStaticEffect>>>();

    // Structure containing all passive effects that aren't directly attached to characters/entities
    Dictionary<EffectTiming.Trigger, List<Passive>> passiveEffects = new Dictionary<EffectTiming.Trigger, List<Passive>>();

    public void Initialize() {
        SharedInstance = this;
    }

    // Creates a new modifier on a character by a certain amount that increments (can be negative) every turn. Default increment is 0 (does not change over time)
    public void AddModifier(Modifier modifier, Character attachee) {
        modifier.timingTriggers.ForEach(trigger => {
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
                    List<AttachedStaticEffect> newList = new List<AttachedStaticEffect>();
                    newList.Add(modifier);
                    attachedEffects[trigger].Add(attachee, newList);
                }
            }
            else {
                // Adds new list of effects if there is not an index for either trigger or attachee
                List<AttachedStaticEffect> newList = new List<AttachedStaticEffect>();
                newList.Add(modifier);
                Dictionary<Character, List<AttachedStaticEffect>> newDictionary = new Dictionary<Character, List<AttachedStaticEffect>>();
                newDictionary.Add(attachee, newList);
                attachedEffects.Add(trigger, newDictionary);
            }
        });
    }

    public void AddStatus(Status status, Character attachee) {
        status.timingTriggers.ForEach(trigger => {
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
                    List<AttachedStaticEffect> newList = new List<AttachedStaticEffect>();
                    newList.Add(status);
                    attachedEffects[trigger].Add(attachee, newList);
                }
            }
            else {
                // Adds new list of effects if there is not an index for either trigger or attachee
                List<AttachedStaticEffect> newList = new List<AttachedStaticEffect>();
                newList.Add(status);
                Dictionary<Character, List<AttachedStaticEffect>> newDictionary = new Dictionary<Character, List<AttachedStaticEffect>>();
                newDictionary.Add(attachee, newList);
                attachedEffects.Add(trigger, newDictionary);
            }
        });
    }

    public void AddPassive(Passive passive) {
        passive.timingTriggers.ForEach(trigger => {
            if (passiveEffects.ContainsKey(trigger)) {
                // Updates current list of effects if there is an index for trigger already
                List<Passive> currentList = passiveEffects[trigger];
                currentList.Add(passive);
                passiveEffects[trigger] = currentList;
            }
            else {
                // Adds new list of effects if there is not an index for trigger
                List<Passive> newList = new List<Passive>();
                newList.Add(passive);
                passiveEffects.Add(trigger, newList);
            }
        });
    }

    public void RemoveModifier(Modifier modifier, Character attachee) {
        modifier.timingTriggers.ForEach(trigger => {
            if (attachedEffects.ContainsKey(trigger) && attachedEffects[trigger].ContainsKey(attachee) && attachedEffects[trigger][attachee].Contains(modifier)) {
                attachedEffects[trigger][attachee].Remove(modifier);
            }
        });
    }

    public void RemoveStatus(Status status, Character attachee) {
        status.timingTriggers.ForEach(trigger => {
            if (attachedEffects.ContainsKey(trigger) && attachedEffects[trigger].ContainsKey(attachee) && attachedEffects[trigger][attachee].Contains(status)) {
                attachedEffects[trigger][attachee].Remove(status);
            }
        });
    }

    public void RemovePassive(Passive passive) {
        passive.timingTriggers.ForEach(trigger => {
            if (passiveEffects.ContainsKey(trigger) && passiveEffects[trigger].Contains(passive)) {
                passiveEffects[trigger].Remove(passive);
            }
        });
    }
}
