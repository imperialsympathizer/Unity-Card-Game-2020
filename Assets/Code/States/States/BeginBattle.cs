using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class BeginBattle : State {
    public BeginBattle(TurnSystem turnSystem) : base(turnSystem) {}

    public override IEnumerator Start() {
        // Do setup for the battle (spawn player, enemies, summons, slots, etc.)
        // ResourceController -> load resources...
        // VisualController -> place resources on screen...
        Debug.Log("beginning battle");
        // After completion, change state to BeginTurn
        TurnSystem.SetState(new BeginTurn(TurnSystem));
        yield break;
    }
}
