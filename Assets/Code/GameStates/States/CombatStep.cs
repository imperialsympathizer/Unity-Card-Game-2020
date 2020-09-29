using System;
using System.Collections;
using UnityEngine;

public class CombatStep : State {
    public CombatStep(TurnSystem turnSystem) : base(turnSystem) { }

    public static event Action<int> OnBeginCombat;
    public static event Action<int> OnEndCombat;

    public override IEnumerator Start() {
        // Combat occurs between player turns, after EndTurn and before BeginTurn (except on turn 1)
        // Every CombatStep, Enemies attack -> CheckGameConditions -> Summons attack -> CheckGameConditions -> player attacks -> CheckGameConditions
        Debug.Log("beginning combat");

        OnBeginCombat?.Invoke(TurnSystem.turnCount);
        CheckGameConditions();

        EnemyAttacks();
        SummonAttacks();
        PlayerAttacks();

        OnEndCombat?.Invoke(TurnSystem.turnCount);
        CheckGameConditions();

        // After completion, change state to BeginTurn
        TurnSystem.SetState(new BeginTurn(TurnSystem));
        yield break;
    }

    private void EnemyAttacks() {
        // Call the enemy controller to perform attacks
        EnemyController.SharedInstance.PerformAttacks();

        // Checks if the game has ended
        CheckGameConditions();
    }

    private void SummonAttacks() {
        SummonController.SharedInstance.PerformAttacks();

        // Checks if the game has ended
        CheckGameConditions();
    }

    private void PlayerAttacks() {
        PlayerController.SharedInstance.PerformAttacks();

        // Checks if the game has ended
        CheckGameConditions();
    }
}
