using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class StaticEffectController : MonoBehaviour {
    public static StaticEffectController SharedInstance;
    // This class manages static effects (both attached and passive) as the game progresses through states

    // Structure containing all passive effects that aren't directly attached to characters/entities
    List<PassiveStaticEffect> passives = new List<PassiveStaticEffect>();
    // Dictionary<string, PassiveStaticEffect> passives = new Dictionary<string, PassiveStaticEffect>();

    // Structure containing all attached effects that are related to characters/entities
    List<AttachedStaticEffect> attached = new List<AttachedStaticEffect>();
    // Dictionary<string, PassiveStaticEffect> attached = new Dictionary<string, PassiveStaticEffect>();

    private void Awake() {
        SharedInstance = this;
    }

    // Creates a new modifier on a character by a certain amount that increments (can be negative) every turn. Default increment is 0 (does not change over time)
    public void AddAttachedModifier(AttachedModifier modifier, Fighter attachee) {
        modifier.character = attachee;
        attachee.AddModifier(modifier);
        attached.Add(modifier);
    }

    public void AddAttachedStatus(AttachedStatus status, Fighter attachee) {
        status.character = attachee;
        attachee.AddStatus(status);
        attached.Add(status);
    }

    public void AddPassiveStatus(PassiveStatus status) {
        passives.Add(status);
    }
}