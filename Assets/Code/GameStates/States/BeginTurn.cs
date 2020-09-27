using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginTurn : State {
    public BeginTurn(TurnSystem turnSystem) : base(turnSystem) { }

    public override IEnumerator Start() {
        // At the start of every turn, resolve any over-time effects
        // Check for death states
        // Then draw 5 new cards
        Debug.Log("beginning turn");

        for (int i = 0; i < 5; i++) {
            CardManager.SharedInstance.DrawCard();
        }

        // After completion, change state to PlayerTurn
        TurnSystem.SetState(new PlayerTurn(TurnSystem));
        yield break;
    }
}
