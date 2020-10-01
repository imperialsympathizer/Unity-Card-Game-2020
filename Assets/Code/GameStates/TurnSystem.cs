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

    public void CheckGameConditions() {
        // This method checks for all win and loss conditions
        // If one is reached, sets the state to EndBattle
        // Otherwise, nothing happens

        // If the player is dead, defeat
        if (PlayerController.GetLife() <= 0 || PlayerController.GetWill() <= 0) {
            Debug.Log("You Lose.");
            GameEndManager.ShowGameEnd(false);
        }

        // If all enemies are dead, win
        if (EnemyController.GetEnemyList().Count < 1) {
            Debug.Log("You Win.");
            GameEndManager.ShowGameEnd(true);
        }
    }
}
