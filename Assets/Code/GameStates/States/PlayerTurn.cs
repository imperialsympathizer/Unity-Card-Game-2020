using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : State {
    private bool endTurn = false;
    private bool cardPlayed = false;

    private bool targetsSelected = false;
    private List<Tuple<int, Target>> targets = new List<Tuple<int, Target>>();

    private int cardId;

    public PlayerTurn(TurnSystem turnSystem) : base(turnSystem) {
        EndTurnButton.OnEndTurnClicked += OnEndTurnButtonClicked;
        CardControl.OnCardPlayed += OnCardPlayed;
    }

    public override IEnumerator Start() {
        // PlayerTurn is when the player is given the opportunity to resolve effects before EndTurn and CombatStep
        // PlayerTurn can theoretically loop to infinity, only breaking when the EndTurn button is clicked by the player
        // PlayerTurn -> CardPlayed -> CheckGameConditions -> PlayerTurn ...

        // Currently the only effect that players can resolve is card plays
        while (!endTurn) {
            if (cardPlayed) {
                TurnSystem.StartCoroutine(ResolveCard());
            }
            yield return new WaitForEndOfFrame();
        }
        // Debug.Log("ending player turn");
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

    private IEnumerator ResolveCard() {
        cardPlayed = false;
        // Get available targets for the card and request player selection
        Card playedCard = CardManager.GetHandCard(cardId);
        if (playedCard != null) {
            // Set the card visual out of the way while targets are chosen (if there are any)
            playedCard.DisableVisual();
            List<DynamicEffect> effects = playedCard.Effects;

            for (int i = 0; i < effects.Count; i++) {
                DynamicEffect effect = effects[i];
                // if the effect targets, request player input to select the targets
                if (effect is TargetableDynamicEffect targetable) {
                    // TargetSelector will enable the targeting canvas and make targetable objects selectable
                    targetsSelected = false;
                    TargetSelector.OnTargetsSelected += OnTargetsSelected;
                    TargetSelector.SharedInstance.EnableTargeting(targetable);
                    while (!targetsSelected) {
                        yield return new WaitForSeconds(0.1f);
                    }
                    // If this is the first effect
                    targetable.selectedTargets = targets;
                    effects[i] = targetable;

                    // Should make a call to TargetSelector for target selection
                }
            }

            // Update the card in hand with the targets, then play it
            playedCard.Effects = effects;
            CardManager.UpdateHandCard(playedCard);
            // TargetSelector.SharedInstance.DisableTargeting();
            TargetSelector.OnTargetsSelected -= OnTargetsSelected;
            CardManager.PlayCard(cardId);
        }

        yield break;
    }

    private void OnTargetsSelected(List<Tuple<int, Target>> targets) {
        this.targets = targets;
        targetsSelected = true;
    }
}
