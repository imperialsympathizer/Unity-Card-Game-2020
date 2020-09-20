using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : State {
    public EndTurn(TurnSystem turnSystem) : base(turnSystem) { }

    public override IEnumerator Start() {
        // EndTurn removes player agency as EOT effects are resolved
        // Discard player hand -> CheckGameConditions -> Resolve end of turn effects -> CheckGameConditions -> CombatStep
        Debug.Log("ending turn");
        EndTurnEffects();
        // After completion, change state to CombatStep
        TurnSystem.SetState(new CombatStep(TurnSystem));
        yield break;
    }

    private void EndTurnEffects() {
        CardManager.SharedInstance.DiscardHand();
    }
}
