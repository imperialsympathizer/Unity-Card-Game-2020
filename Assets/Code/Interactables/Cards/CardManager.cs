using System;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardManager {
    // This class is for the management of cards through drawing/discarding and playing
    // It delegates responsiblities to the Deck, Hand, and Discard for moving objects between them
    // It also triggers visuals on cards (e.g. they appear on screen) when relevant
    public static CardManager SharedInstance;

    // The library of cards that can be played with
    private CardSource cardSource = new CardSource();

    private Deck deck = new Deck();
    private Hand hand = new Hand();
    private Discard discard = new Discard();

    private CurvedLayout handArea;
    private TextMeshProUGUI deckCount;
    private TextMeshProUGUI discardCount;

    public void Initialize() {
        SharedInstance = this;

        cardSource.InitializeCards();
        deck.Initialize(cardSource.allCards);
        discard.Initialize();

        handArea = VisualController.SharedInstance.GetHand().GetComponent<CurvedLayout>();
        deckCount = VisualController.SharedInstance.GetDeckCount().GetComponent<TextMeshProUGUI>();
        discardCount = VisualController.SharedInstance.GetDiscardCount().GetComponent<TextMeshProUGUI>();
    }

    public void PlayCard(int cardId) {
        // Update life and will from the card's cost through the PlayerController
        // Remove the card from hand
        Card playedCard = hand.RemoveCard(cardId);

        // Get all required targets for the card from the player
        if (playedCard.SetTargets()) {

        }
        else {
            // If the player cancels the card while choosing targets/actions, return it to hand and do not play the card
            hand.AddCard(playedCard);
            UpdateVisuals();
        }


        // If there was a matching card in the hand, calculate resulting life and will (the player can kill themselves)
        // Then, clear the visuals and add the card to the discard
        // Then, push any DynamicEffects related to the card to the DynamicEffectController queue
        // Effects of the card in question are resolved in a FIFO order
        if (playedCard != null) {
            // Calculate resulting life/will
            PlayerController.SharedInstance.GetLife();
            int lifeResult = PlayerController.SharedInstance.GetLife() - playedCard.lifeCost;
            int willResult = PlayerController.SharedInstance.GetWill();
            while (lifeResult < 1) {
                lifeResult += 20;
                willResult--;
            }

            // Adjust life and will totals
            PlayerController.SharedInstance.SetLife(lifeResult);
            PlayerController.SharedInstance.SetWill(willResult);

            // Move card to discard and disable visual
            playedCard.ClearVisual();
            discard.AddCard(playedCard);
            DynamicEffectController.SharedInstance.AddEffects(playedCard.Effects);

            // Update visuals on screen
            PlayerController.SharedInstance.UpdateVisual();
            UpdateVisuals();
        }
    }

    public void DrawCard() {
        // Check max hand size to see if a card can be drawn 
        if (hand.CanDraw()) {
            // Check if the deck has cards in it
            if (deck.GetSize() < 1) {
                // If the deck has 0 (or fewer?) cards in it, reshuffle the discard
                deck.AddCards(discard.GetCards());
                discard.ClearCards();
                deck.Shuffle();
            }

            Card drawnCard = deck.DrawCard();
            hand.AddCard(drawnCard);
            drawnCard.CreateVisual();
            UpdateVisuals();
        }
    }

    public void DiscardRandomCard() {
        // When no card or index is given, a card is discarded at random

        UpdateVisuals();
    }

    public void DiscardCard(int cardId) {

        UpdateVisuals();
    }

    public void DiscardHand() {
        // This method is usually called at the end of the turn, but could be used in other circumstances
        List<Card> discarded = hand.GetCards();
        for (int i = 0; i <discarded.Count; i++) {
            Card card = discarded[i];
            if (card != null) {
                card.ClearVisual();
            }
            else {
                discarded.RemoveAt(i);
            }
        }
        discard.AddCards(discarded);
        hand.ClearHand();
        UpdateVisuals();
    }

    public void ReturnDiscardToDeck() {

    }

    private void UpdateVisuals() {
        // Updates any visuals that display hand, deck, or discard values
        deckCount.text = deck.GetSize().ToString();
        discardCount.text = discard.GetSize().ToString();

        handArea.UpdateCardPositions();
    }
}
