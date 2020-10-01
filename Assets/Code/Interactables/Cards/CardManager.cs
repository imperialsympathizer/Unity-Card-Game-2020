using System;
using System.Collections.Generic;
using TMPro;

public static class CardManager {
    // This class is for the management of cards through drawing/discarding and playing
    // It delegates responsiblities to the Deck, Hand, and Discard for moving objects between them
    // It also triggers visuals on cards (e.g. they appear on screen) when relevant
    // The library of cards that can be played with
    private static CardSource cardSource = new CardSource();

    private static Deck deck = new Deck();
    private static Hand hand = new Hand();
    private static Discard discard = new Discard();

    private static CurvedLayout handArea;
    private static TextMeshProUGUI deckCount;
    private static TextMeshProUGUI discardCount;

    public static event Action<Card> OnCardDraw;
    public static event Action<Card> OnCardPlay;
    public static event Action<Card> OnDiscard;

    public static void Initialize() {
        cardSource.InitializeCards();
        deck.Initialize(cardSource.allCards);
        discard.Initialize();

        handArea = VisualController.SharedInstance.GetHand().GetComponent<CurvedLayout>();
        deckCount = VisualController.SharedInstance.GetDeckCount().GetComponent<TextMeshProUGUI>();
        discardCount = VisualController.SharedInstance.GetDiscardCount().GetComponent<TextMeshProUGUI>();
    }

    public static void PlayCard(int cardId) {
        // Update life and will from the card's cost through the PlayerController
        // Remove the card from hand
        Card playedCard = hand.RemoveCard(cardId);

        // Get all required targets for the card from the player
        // TODO
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
            // Adjust life and will totals
            PlayerController.UpdateLife(-playedCard.lifeCost);

            // Move card to discard and disable visual
            playedCard.ClearVisual();
            discard.AddCard(playedCard);
            DynamicEffectController.SharedInstance.AddEffects(playedCard.Effects);

            // Update visuals on screen
            PlayerController.UpdateVisual();
            UpdateVisuals();

            // Fire card played event
            OnCardPlay?.Invoke(playedCard);
        }
    }

    public static void DrawCard() {
        // Check max hand size to see if a card can be drawn 
        if (hand.CanDraw()) {
            // Check if the deck has cards in it
            if (deck.GetSize() < 1) {
                // If the deck has 0 (or fewer?) cards in it, reshuffle the discard
                ReturnDiscardToDeck();
            }

            Card drawnCard = deck.DrawCard();
            hand.AddCard(drawnCard);
            drawnCard.CreateVisual();
            UpdateVisuals();

            // Fire card drawn event
            OnCardPlay?.Invoke(drawnCard);
        }
    }

    public static void DiscardRandomCard() {
        // When no card or index is given, a card is discarded at random
        // TODO
        UpdateVisuals();

        // Fire card played event
        // OnDiscard?.Invoke(discardedCard);
    }

    public static void DiscardCard(int cardId) {
        // TODO
        UpdateVisuals();

        // Fire card played event
        // OnDiscard?.Invoke(discardedCard);
    }

    public static void DiscardHand() {
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

    public static void ReturnDiscardToDeck() {
        deck.AddCards(discard.GetCards());
        discard.ClearCards();
        deck.Shuffle();
    }

    private static void UpdateVisuals() {
        // Updates any visuals that display hand, deck, or discard values
        deckCount.text = deck.GetSize().ToString();
        discardCount.text = discard.GetSize().ToString();

        handArea.UpdateCardPositions();
    }
}
