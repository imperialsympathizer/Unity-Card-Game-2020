using System;
using System.Collections;

public class BeginBattle : State {
    public BeginBattle(TurnSystem turnSystem) : base(turnSystem) { }

    public static event Action OnBeginBattle;

    public override IEnumerator Start() {
        // Debug.Log("beginning battle");

        OnBeginBattle?.Invoke();

        if (!CheckGameConditions()) {
            // After completion, change state to BeginTurn
            TurnSystem.SetState(new BeginTurn(TurnSystem));
        }
        yield break;
    }
}
