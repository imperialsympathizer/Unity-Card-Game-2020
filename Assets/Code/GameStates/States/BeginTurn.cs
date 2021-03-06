﻿using System;
using System.Collections;

public class BeginTurn : State {
    public BeginTurn(TurnSystem turnSystem) : base(turnSystem) { }

    public static event Action<int> OnBeginTurn;

    public static void ClearSubscriptions() {
        OnBeginTurn = null;
    }

    public override IEnumerator Start() {
        // At the start of every turn, draw 5 cards
        // Then, resolve any over-time effects
        // Then, reset the player's life to max and lose will accordingly
        // Check for death statess
        // Debug.Log("beginning turn");

        TurnSystem.turnCount++;
        for (int i = 0; i < 5; i++) {
            CardManager.Instance.DrawCard();
        }

        if (!CheckGameConditions()) {
            OnBeginTurn?.Invoke(TurnSystem.turnCount);
            // Reset available life to spend
            PlayerController.Instance.ResetVigor();

            if (!CheckGameConditions()) {
                // After completion, change state to PlayerTurn
                TurnSystem.SetState(new PlayerTurn(TurnSystem));
            }
        }
        
        yield break;
    }
}
