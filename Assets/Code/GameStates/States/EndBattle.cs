using System;
using System.Collections;

public class EndBattle : State {
    public EndBattle(TurnSystem turnSystem) : base(turnSystem) { }

    public static event Action<int> OnBattleEnd;

    public override IEnumerator Start() {
        // EndBattle is only triggered when all enemies are defeated, the player is defeated, or another trigger which would cause a win/loss
        // This state can only be reached by the CheckGameConditions state
        // PlayerTurn can theoretically loop to infinity, only breaking when the EndTurn button is clicked by the player
        // Debug.Log("ending battle");

        OnBattleEnd?.Invoke(TurnSystem.turnCount);
        // After completion, change state to EndTurn
        // TurnSystem.SetState(new BeginTurn(TurnSystem));
        yield break;
    }
}
