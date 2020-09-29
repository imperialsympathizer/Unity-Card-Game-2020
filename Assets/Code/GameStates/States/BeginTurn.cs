using System;
using System.Collections;
using UnityEngine;

public class BeginTurn : State {
    public BeginTurn(TurnSystem turnSystem) : base(turnSystem) { }

    public static event Action<int> OnBeginTurn;

    public override IEnumerator Start() {
        // At the start of every turn, resolve any over-time effects
        // Check for death states
        // Then draw 5 new cards
        Debug.Log("beginning turn");

        TurnSystem.turnCount++;
        OnBeginTurn?.Invoke(TurnSystem.turnCount);
        CheckGameConditions();

        for (int i = 0; i < 5; i++) {
            CardManager.SharedInstance.DrawCard();
        }

        CheckGameConditions();

        // After completion, change state to PlayerTurn
        TurnSystem.SetState(new PlayerTurn(TurnSystem));
        yield break;
    }
}
