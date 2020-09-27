using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State {
    protected TurnSystem TurnSystem;

    public State(TurnSystem turnSystem) {
        TurnSystem = turnSystem;
    }

    public virtual IEnumerator Start() {
        yield break;
    }

    public void CheckGameConditions() {
        // This method checks for all win and loss conditions
        // If one is reached, sets the state to EndBattle
        // Otherwise, nothing happens

        // If the player is dead, defeat
        if (PlayerController.SharedInstance.GetLife() <= 0 || PlayerController.SharedInstance.GetWill() <= 0) {
            Debug.Log("You Lose.");
            GameEndManager.SharedInstance.ShowGameEnd(false);
        }

        // If all enemies are dead, win
        if (EnemyController.SharedInstance.GetEnemyList().Count < 1) {
            Debug.Log("You Win.");
            GameEndManager.SharedInstance.ShowGameEnd(true);
        }
    }
}
