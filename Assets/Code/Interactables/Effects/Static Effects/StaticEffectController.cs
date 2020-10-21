using System.Collections.Generic;

public class StaticEffectController : BaseController {
    // This class manages static effects (both attached and passive) as the game progresses through states
    public static StaticEffectController Instance;

    // Structure containing all attached effects that are related to characters/entities (Statuses and Modifiers)
    // Key - effect Id, Value - effect
    private Dictionary<int, AttachedStaticEffect> attachedEffects = new Dictionary<int, AttachedStaticEffect>();

    // Structure containing all passive effects that aren't directly attached to characters/entities
    private Dictionary<int, Passive> passiveEffects = new Dictionary<int, Passive>();

    protected override bool Initialize() {
        Instance = this;
        return true;
    }

    #region Data Modification

    // Creates a new modifier on a character
    // Adds the modifier as a dictionary entry for every trigger that it has
    public void AddModifier(Character character, Modifier modifier) {
        if (character != null && modifier != null) {
            modifier.AttachEffectToCharacter(character);
            attachedEffects.Add(modifier.id, modifier);
        }
    }

    public void AddStatus(Character character, Status status) {
        if (character != null && status != null) {
            status.AttachEffectToCharacter(character);
            attachedEffects.Add(status.id, status);
        }
    }

    public void AddPassive(Passive passive) {
        if (passive != null) {
            passive.Activate();
            passiveEffects.Add(passive.id, passive);
        }
    }

    public void RemoveModifier(int modifierId) {
        if (attachedEffects.TryGetValue(modifierId, out AttachedStaticEffect effect)) {
            effect.RemoveEffectFromCharacter();
            attachedEffects.Remove(modifierId);
        }
    }

    public void RemoveStatus(int statusId) {
        if (attachedEffects.TryGetValue(statusId, out AttachedStaticEffect effect)) {
            effect.RemoveEffectFromCharacter();
            attachedEffects.Remove(statusId);
        }
    }

    public void RemovePassive(int passiveId) {
        if (passiveEffects.TryGetValue(passiveId, out Passive effect)) {
            attachedEffects.Remove(passiveId);
        }
    }

    #endregion
}
