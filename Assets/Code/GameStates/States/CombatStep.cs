using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStep : State {
    public CombatStep(TurnSystem turnSystem) : base(turnSystem) { }

    public static event Action<int> OnBeginCombat;
    public static event Action<int> OnEndCombat;

    private bool combatOver = false;
    private bool animationComplete = false;

    public override IEnumerator Start() {
        // Combat occurs between player turns, after EndTurn and before BeginTurn (except on turn 1)
        // Every CombatStep, Enemies attack -> CheckGameConditions -> Summons attack -> CheckGameConditions -> player attacks -> CheckGameConditions
        // Debug.Log("beginning combat");

        OnBeginCombat?.Invoke(TurnSystem.turnCount);
        CheckGameConditions();

        // Initiate combat and wait for end
        combatOver = false;
        AttackAnimator.OnAnimateComplete += OnAnimateComplete;
        TurnSystem.StartCoroutine(EnemyAttacks());

        while (!combatOver) {
            yield return new WaitForSeconds(0.1f);
        }

        OnEndCombat?.Invoke(TurnSystem.turnCount);
        CheckGameConditions();

        // After completion, change state to BeginTurn
        TurnSystem.SetState(new BeginTurn(TurnSystem));
        yield break;
    }

    private IEnumerator EnemyAttacks() {
        // Get the list of enemies
        List<Enemy> enemies = EnemyController.Instance.GetEnemyList();

        // Iterate from left to right
        for (int i = 0; i < enemies.Count; i++) {
            Enemy attacker = enemies[i];
            if (attacker.AttackTimes > 0 && attacker.AttackValue > 0) {
                Fighter defender;

                // Resolve attacks on the front summon or player until it is dead, then continue until all attacks are gone or the player is defeated
                for (int j = 0; j < attacker.AttackTimes; j++) {
                    bool summon = true;
                    // If there is a summon available to be hit by an attack, attack it
                    defender = SummonController.Instance.GetDefender();
                    if (defender == null) {
                        summon = false;
                        // Otherwise, attack the player
                        defender = PlayerController.Instance.GetPlayer();
                        if (defender == null) {
                            // If there are no entities to resolve attacks on (they're dead) escape the loop
                            break;
                        }
                    }

                    // Perform attack animation and await completion
                    animationComplete = false;
                    attacker.PerformAttack();
                    while (!animationComplete) {
                        yield return new WaitForSeconds(0.1f);
                    }

                    // Change the life total to reflect damage taken
                    if (summon) {
                        SummonController.Instance.CompleteAttack(defender.id, attacker);
                    }
                    else {
                        PlayerController.Instance.CompleteAttack(attacker);
                    }

                    // wait for health to decrease before the next attack
                    yield return new WaitForSeconds(0.3f);

                    // Checks if the game has ended
                    CheckGameConditions();
                }
            }
        }

        TurnSystem.StartCoroutine(SummonAttacks());
        yield break;
    }

    private IEnumerator SummonAttacks() {
        // Get the list of enemies
        List<Summon> summons = SummonController.Instance.GetSummonList();

        // Iterate from left to right
        for (int i = summons.Count - 1; i >= 0; i--) {
            Summon attacker = summons[i];
            if (attacker.AttackTimes > 0 && attacker.AttackValue > 0) {
                Fighter defender;

                // Resolve attacks on the front enemy until it is dead, then continue until all attacks are gone or all enemies are defeated
                for (int j = 0; j < attacker.AttackTimes; j++) {
                    // If there is an enemy available to be hit by an attack, attack it
                    defender = EnemyController.Instance.GetDefender();
                    if (defender == null) {
                        // If there are no enemies to resolve attacks on (they're dead) escape the loop
                        break;
                    }

                    // Perform attack animation and await completion
                    animationComplete = false;
                    attacker.PerformAttack();
                    while (!animationComplete) {
                        yield return new WaitForSeconds(0.1f);
                    }

                    // Change the life total to reflect damage taken
                    EnemyController.Instance.CompleteAttack(defender.id, attacker);

                    // wait for health to decrease before the next attack
                    yield return new WaitForSeconds(0.3f);

                    // Checks if the game has ended
                    CheckGameConditions();
                }
            }
        }

        TurnSystem.StartCoroutine(PlayerAttacks());
        yield break;
    }

    private IEnumerator PlayerAttacks() {
        // Get the list of enemies
        Player player = PlayerController.Instance.GetPlayer();

        if (player.AttackTimes > 0 && player.AttackValue > 0) {
            Fighter defender;

            // Resolve attacks on the front enemy until it is dead, then continue until all attacks are gone or all enemies are defeated
            for (int j = 0; j < player.AttackTimes; j++) {
                // If there is an enemy available to be hit by an attack, attack it
                defender = EnemyController.Instance.GetDefender();
                if (defender == null) {
                    // If there are no enemies to resolve attacks on (they're dead) escape the loop
                    break;
                }

                // Perform attack animation and await completion
                animationComplete = false;
                player.PerformAttack();
                while (!animationComplete) {
                    yield return new WaitForSeconds(0.1f);
                }

                // Change the life total to reflect damage taken
                EnemyController.Instance.CompleteAttack(defender.id, player);

                // wait for health to decrease before the next attack
                yield return new WaitForSeconds(0.3f);

                // Checks if the game has ended
                CheckGameConditions();
            }
        }

        combatOver = true;
        yield break;
    }

    private void OnAnimateComplete() {
        animationComplete = true;
    }
}
