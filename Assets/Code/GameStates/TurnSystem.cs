using System.Collections;
using UnityEngine;

public class TurnSystem : StateMachine {
    public static TurnSystem SharedInstance;

    public static int turnCount = 0;

    void Start() {
        SharedInstance = this;
        // Will loop waiting for resources to load and then start the game
        StartCoroutine(AwaitStart());
    }

    private IEnumerator AwaitStart() {
        // Ensure resources are loaded before starting the battle
        while (!ResourceController.Loaded) {
            yield return new WaitForEndOfFrame();
        }

        SetState(new BeginBattle(this));
    }

    public bool IsPlayerTurn() {
        return (State.GetType() == typeof(PlayerTurn));
    }

    public bool CheckGameConditions() {
        // This method checks for all win and loss conditions
        // If one is reached, sets the state to EndBattle
        // Otherwise, nothing happens

        // If the player has no will left, defeat
        if (PlayerController.GetWill() <= 0) {
            // Debug.Log("You Lose.");
            GameEndManager.SharedInstance.ShowGameEnd(false);
            return true;
        }

        // If all enemies are dead, win
        if (EnemyController.GetEnemyList().Count < 1) {
            // Debug.Log("You Win.");
            GameEndManager.SharedInstance.ShowGameEnd(true);
            return true;
        }

        return false;
    }
}
