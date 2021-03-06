﻿using System;
using System.Collections;

public class EndTurn : State {
    public EndTurn(TurnSystem turnSystem) : base(turnSystem) { }

    public static event Action<int> OnEndTurn;

    public static void ClearSubscriptions() {
        OnEndTurn = null;
    }

    public override IEnumerator Start() {
        // EndTurn removes player agency as EOT effects are resolved
        // Discard player hand -> CheckGameConditions -> Resolve end of turn effects -> CheckGameConditions -> CombatStep
        // Debug.Log("ending turn");

        EndTurnEffects();
        if (!CheckGameConditions()) {
            OnEndTurn?.Invoke(TurnSystem.turnCount);

            if (!CheckGameConditions()) {
                // After completion, change state to BeginTurn
                TurnSystem.SetState(new BeginTurn(TurnSystem));
            }
        }

        yield break;
    }

    private void EndTurnEffects() {
        CardManager.Instance.DiscardHand();
    }
}
