using System.Collections.Generic;

public class Infector : Status {
    // This status goes on characters that deal infection when they attack
    // Additionally, on the character's death, it will trigger infection if killed by an enemy
    // infectionValue: The amount of infection that is applied to a character on a specific trigger (can be any character, not necessarily the character with the Infector status)

    public Infector(int effectCount) :
        base(
        effectCount,
        new List<Trigger> { // List of triggers for the effect
            new OnDamageAttack(new List<Trigger.TriggerAction> { // Subscribe to the OnDamageAttack trigger
                Trigger.TriggerAction.RESOLVE // Execute ResolveEffect() when triggered
            })
        },
        0) {}

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
        // If the character already has infection status, increase it by effectCount
        // Otherwise, create a new infection Status equal to effectCount

        bool infected = false;
        foreach (KeyValuePair<int, AttachedStaticEffect> attachedEffect in character.attachedEffects) {
            if (attachedEffect.Value is Infected infectedStatus) {
                infectedStatus.effectCount += effectCount;
                character.attachedEffects[attachedEffect.Key] = infectedStatus;
                infected = true;
                break;
            }
        }

        // If there wasn't an infection status on the character, create one
        if (!infected) {
            StaticEffectController.AddStatus(character, new Infected(effectCount));
        }
    }
}
