using System;
using System.Collections;

public class EndBattle : State {
    private bool victory;

    public EndBattle(TurnSystem turnSystem, bool victory) : base(turnSystem) {
        this.victory = victory;
    }

    public static event Action<int, bool> OnEndBattle;

    public static void ClearSubscriptions() {
        OnEndBattle = null;
    }

    public override IEnumerator Start() {
        // EndBattle is only triggered when all enemies are defeated, the player is defeated, or another trigger which would cause a win/loss
        // This state can only be reached by the CheckGameConditions state
        // PlayerTurn can theoretically loop to infinity, only breaking when the EndTurn button is clicked by the player
        // Debug.Log("ending battle");

        OnEndBattle?.Invoke(TurnSystem.turnCount, victory);
        yield break;
    }
}
