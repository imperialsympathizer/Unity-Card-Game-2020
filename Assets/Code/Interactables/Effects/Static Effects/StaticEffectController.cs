﻿using System.Collections.Generic;
using UnityEngine;

public static class StaticEffectController {
    // This class manages static effects (both attached and passive) as the game progresses through states

    // Structure containing all attached effects that are related to characters/entities (Statuses and Modifiers)
    // Sorted by trigger first, then character
    // TODO: worth considering to sort by trigger > priority > character instead of trigger > character > priority, but that's more of a high level design decision for later
    private static Dictionary<int, AttachedStaticEffect> attachedEffects = new Dictionary<int, AttachedStaticEffect>();

    // Structure containing all passive effects that aren't directly attached to characters/entities
    private static Dictionary<int, Passive> passiveEffects = new Dictionary<int, Passive>();

    public static void Initialize() {
    }

    #region Data Modification

    // Creates a new modifier on a character
    // Adds the modifier as a dictionary entry for every trigger that it has
    public static void AddModifier(Modifier modifier) {
        // Debug.Log($"Adding modifier: {modifier.modifierType}");
        attachedEffects.Add(modifier.id, modifier);
        modifier.AttachEffectToCharacter();
    }

    public static void AddStatus(Status status) {
        // Debug.Log($"Adding status: {status.statusType}");
        attachedEffects.Add(status.id, status);
        status.AttachEffectToCharacter();
    }

    public static void AddPassive(Passive passive) {
        // Debug.Log($"Adding passive: {passive.passiveType}");
        passiveEffects.Add(passive.id, passive);
    }

    public static void RemoveModifier(int modifierId) {
        if (attachedEffects.TryGetValue(modifierId, out AttachedStaticEffect effect)) {
            effect.RemoveEffectFromCharacter();
            attachedEffects.Remove(modifierId);
        }
    }

    public static void RemoveStatus(int statusId) {
        if (attachedEffects.TryGetValue(statusId, out AttachedStaticEffect effect)) {
            effect.RemoveEffectFromCharacter();
            attachedEffects.Remove(statusId);
        }
    }

    public static void RemovePassive(int passiveId) {
        if (passiveEffects.TryGetValue(passiveId, out Passive effect)) {
            attachedEffects.Remove(passiveId);
        }
    }

    #endregion
}
