using System.Collections.Generic;

public class Infector : Status {
    // This status goes on characters that deal infection when they attack
    // Additionally, on the character's death, it will trigger infection if killed by an enemy

    // The amount of infection that is applied to a character on a specific trigger (can be any character, not necessarily the character with the Infector status)
    private int infectionValue;

    public Infector(Character character, int infectionValue) :
        base(
        character,
        StatusType.INFECTOR,
        1,
        new List<Trigger> { // List of triggers for the effect
            new OnDamageAttack(new List<Trigger.TriggerAction> { // Subscribe to the OnDamageAttack trigger
                Trigger.TriggerAction.RESOLVE // Execute ResolveEffect() when triggered
            })
        },
        0) {
        this.infectionValue = infectionValue;
    }

    protected override void ResolveEffect(Trigger trigger) {
        if (trigger is OnDamageAttack onDmg) {
            // On an attack event where the character is the attacker, apply infection equal to infectionValue to the defender
            if (onDmg.attacker == this.character) {
                InfectCharacter(onDmg.defender);
            }
            else if (onDmg.defender == this.character && onDmg.lifeResult <= 0) {
                // Otherwise, apply infection if the character is the defender and is killed by the attacker
                InfectCharacter(onDmg.attacker);
            }
        }
    }

    protected override void OperateOnEffect(Trigger trigger) {}

    private void InfectCharacter(Character character) {
        // Debug.Log($"Infecting character: {character.name}");
        // If the character already has infection status, increase it by infectionValue
        // Otherwise, create a new infectionStatus equal to infectionValue

        bool infected = false;
        foreach (KeyValuePair<int, AttachedStaticEffect> attachedEffect in character.attachedEffects) {
            if (attachedEffect.Value is Infected infectedStatus) {
                infectedStatus.infectionValue += infectionValue;
                character.attachedEffects[attachedEffect.Key] = infectedStatus;
                infected = true;
                break;
            }
        }

        // If there wasn't an infection status on the character, create one
        if (!infected) {
            StaticEffectController.AddStatus(new Infected(character, infectionValue));
        }
    }
}
