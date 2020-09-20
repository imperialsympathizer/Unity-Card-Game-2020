using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class TurnSystem : StateMachine {
    public static TurnSystem SharedInstance;

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
}
