using System;
using System.Collections;
using UnityEngine;

public class BeginBattle : State {
    public BeginBattle(TurnSystem turnSystem) : base(turnSystem) {}

    public static event Action OnBeginBattle;

    public override IEnumerator Start() {
        Debug.Log("beginning battle");

        OnBeginBattle?.Invoke();

        CheckGameConditions();

        // After completion, change state to BeginTurn
        TurnSystem.SetState(new BeginTurn(TurnSystem));
        yield break;
    }
}
