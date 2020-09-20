using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : State {
    private bool endTurn = false;
    private bool cardPlayed = false;

    private int cardId;

    public PlayerTurn(TurnSystem turnSystem) : base(turnSystem) {
        EndTurnButton.OnEndTurnClicked += OnEndTurnButtonClicked;
        CardMouseInteraction.OnCardPlayed += OnCardPlayed;
    }

    public override IEnumerator Start() {
        // PlayerTurn is when the player is given the opportunity to resolve effects before EndTurn and CombatStep
        // PlayerTurn can theoretically loop to infinity, only breaking when the EndTurn button is clicked by the player
        // PlayerTurn -> CardPlayed -> CheckGameConditions -> PlayerTurn ...

        // Currently the only effect that players can resolve is card plays
        while (!endTurn) {
            if (cardPlayed) {
                ResolveCard();
                CheckGameConditions();
            }
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("ending player turn");
        // After completion, change state to EndTurn
        TurnSystem.SetState(new EndTurn(TurnSystem));
        yield break;
    }

    private void OnEndTurnButtonClicked() {
        endTurn = true;
    }

    private void OnCardPlayed(int cardId) {
        this.cardId = cardId;
        cardPlayed = true;
    }

    private void ResolveCard() {
        cardPlayed = false;
        CardManager.SharedInstance.PlayCard(cardId);
    }
}
