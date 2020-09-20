using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStep : State {
    public CombatStep(TurnSystem turnSystem) : base(turnSystem) { }

    public override IEnumerator Start() {
        // Combat occurs between player turns, after EndTurn and before BeginTurn (except on turn 1)
        // Every CombatStep, Enemies attack -> CheckGameConditions -> Summons attack -> CheckGameConditions -> player attacks -> CheckGameConditions
        Debug.Log("beginning combat");

        EnemyAttacks();
        SummonAttacks();
        PlayerAttacks();

        // After completion, change state to BeginTurn
        TurnSystem.SetState(new BeginTurn(TurnSystem));
        yield break;
    }

    private void EnemyAttacks() {
        // Retrieve the queue of attacks
        Queue<int> attacks = EnemyController.SharedInstance.GetAttackQueue();

        // Resolve attacks on the front summon or player until it is dead, then continue until all attacks are gone or the player is defeated
        if (attacks != null) {
            while (attacks.Count > 0) {
                int attack = attacks.Dequeue();
                // If there is not a summon available to be hit by an attack, attack the player
                // Otherwise, the attack is resolved against a summon in the if statement itself
                if (!SummonController.SharedInstance.ResolveAttack(attack)) {
                    // If there are no players to resolve attacks on (they're dead) escape the loop
                    if (!PlayerController.SharedInstance.ResolveAttack(attack)) {
                        break;
                    }
                }
            }
        }

        // Checks if the game has ended
        CheckGameConditions();
    }

    private void SummonAttacks() {
        Queue<int> attacks = SummonController.SharedInstance.GetAttackQueue();

        // Resolve attacks on the front enemy until it is dead, then continue until all attacks are gone or the player is defeated
        if (attacks != null) {
            while (attacks.Count > 0) {
                int attack = attacks.Dequeue();
                if (!EnemyController.SharedInstance.ResolveAttack(attack)) {
                    // If there are no enemies to resolve attacks on escape the loop
                    break;
                }
            }
        }

        // Checks if the game has ended
        CheckGameConditions();
    }

    private void PlayerAttacks() {
        Queue<int> attacks = PlayerController.SharedInstance.GetAttackQueue();

        // Resolve attacks on the front enemy until it is dead, then continue until all attacks are gone or the player is defeated
        if (attacks != null) {
            while (attacks.Count > 0) {
                int attack = attacks.Dequeue();
                if (!EnemyController.SharedInstance.ResolveAttack(attack)) {
                    // If there are no enemies to resolve attacks on escape the loop
                    break;
                }
            }
        }

        // Checks if the game has ended
        CheckGameConditions();
    }
}
