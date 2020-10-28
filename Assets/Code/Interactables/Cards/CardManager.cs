using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CardManager : MonoBehaviour {
    // This class is for the management of cards through drawing/discarding and playing
    // It delegates responsiblities to the Deck, Hand, and Discard for moving objects between them
    // It also triggers visuals on cards (e.g. they appear on screen) when relevant
    public static CardManager Instance;

    // The library of all cards
    private CardSource cardSource;

    // Cards existing in the player's deck during a run
    private Deck runDeck;

    // Card containers for battle
    private Deck deck;
    private Hand hand;
    private Discard discard;
    private Exile exile;

    private CurvedLayout handArea;
    private TextMeshProUGUI deckCount;
    private TextMeshProUGUI discardCount;

    // Variables for managing card plays/targeting since they involve asynchronous tasks
    private Card playedCard;
    private bool targetSelected;
    private List<Tuple<int, Target>> effectTargets = new List<Tuple<int, Target>>();

    public bool Initialized { get; private set; } = false;


    public static event Action<Card> OnCardDraw;
    public static event Action<Card> OnCardPlay;
    public static event Action<Card> OnDiscard;

    public static void ClearSubscriptions() {
        OnCardDraw = null;
        OnCardPlay = null;
        OnDiscard = null;
    }

    private void Awake() {
        Instance = this;
    }

    public IEnumerator Initialize(bool reinitialize) {
        if (!Initialized || reinitialize) {
            // Wait for VisualController to be initialized
            while (VisualController.Instance == null || !VisualController.Instance.Initialized ||
                RewardController.Instance == null || !RewardController.Instance.Initialized) {
                yield return null;
            }

            cardSource = new CardSource();
            if (!reinitialize) {
                // Create the starter deck
                runDeck = new Deck(cardSource.allCards);
            }
            else {
                runDeck = new Deck(ResourceController.runDeck);
            }

            deck = new Deck(runDeck);
            discard = new Discard();
            hand = new Hand();
            exile = new Exile();

            deck.Shuffle();

            handArea = VisualController.Instance.GetHand().GetComponent<CurvedLayout>();
            deckCount = VisualController.Instance.GetDeckCount().GetComponent<TextMeshProUGUI>();
            discardCount = VisualController.Instance.GetDiscardCount().GetComponent<TextMeshProUGUI>();

            Initialized = true;
        }
    }

    public void ResetInitialization() {
        Initialized = false;
    }

    public Card GenerateRewardCard(Dictionary<string, Card> cardsNot) {
        // Determine card rarity from RewardController
        Rarity rarity = RewardController.Instance.GenerateCardRarity();
        // Filter the list of possible card rewards (specific rarity, no overlapping names with cardsNot)
        Card[] possibleCards = cardSource.allCards.Select(kv => kv.Value).Where(card => (card.rarity == rarity && !cardsNot.ContainsKey(card.name))).ToArray();
        // Return the card at a random index in possibleCards
        return possibleCards[RandomNumberGenerator.Instance.GetRandomIntFromRange(possibleCards.Length)];
    }

    public List<Card> GetHandCards() {
        return hand.GetCards();
    }

    public void AddCardToRunDeck(Card chosenCard) {
        runDeck.AddCard(chosenCard);
    }

    public Card GetHandCardById(int cardId) {
        return hand.GetCard(cardId);
    }

    public void UpdateHandCard(Card updatedCard) {
        hand.UpdateCard(updatedCard);
    }

    public List<Card> GetRunDeckList() {
        return runDeck.GetCards();
    }

    public Deck GetRunDeck() {
        return runDeck;
    }

    #region PlayCard
    public void BeginCardPlay(int cardId) {
        playedCard = hand.GetCard(cardId);
        if (playedCard != null) {
            // Only play the card if the player can afford the life cost
            if (PlayerController.Instance.GetVigor() >= playedCard.LifeCost) {
                // Set the card visual out of the way temporarily
                playedCard.EnableVisual(false);

                // Resolve card effects
                StartCoroutine(ResolveCardEffects());
            }
            else {
                playedCard.ReturnToHand();
                UpdateVisuals();
            }
        }
    }

    private IEnumerator ResolveCardEffects() {
        // Loop through all card effects and resolve them
        List<DynamicEffect> effects = playedCard.effects;
        bool cardPlayed = true;
        for (int i = 0; i < effects.Count; i++) {
            DynamicEffect effect = effects[i];
            // Request target selection from the player based on available targets (if effect targets)
            if (effect is TargetableDynamicEffect targetable) {
                // TargetSelector will enable the targeting canvas and make targetable objects selectable
                targetSelected = false;
                TargetSelector.OnTargetingComplete += OnTargetingComplete;
                TargetSelector.Instance.EnableTargeting(targetable);
                while (!targetSelected) {
                    yield return new WaitForSeconds(0.1f);
                }
                // Update effects with targets
                // If effectTargets is null, target selection was cancelled -> cancel card play
                if (effectTargets != null) {
                    targetable.selectedTargets = effectTargets;
                    effects[i] = targetable;
                }
                else {
                    cardPlayed = false;
                    break;
                }

                // Unsubscribe from TargetingComplete
                TargetSelector.OnTargetingComplete -= OnTargetingComplete;
            }
        }

        // Update the card in hand with the targets, then play it
        playedCard.effects = effects;
        FinishCardPlay(cardPlayed);
        yield break;
    }

    private void OnTargetingComplete(List<Tuple<int, Target>> effectTargets) {
        targetSelected = true;
        this.effectTargets = effectTargets;
    }

    private void FinishCardPlay(bool cardPlayed) {
        // If all targets and options were successfully chosen by the player, finish playing the card
        if (cardPlayed) {
            // Remove the card from hand
            hand.RemoveCard(playedCard.Id);

            // Calculate resulting life and will (the player can kill themselves)
            PlayerController.Instance.UpdateVigor(-playedCard.LifeCost);
            PlayerController.Instance.UpdateLife(-playedCard.LifeCost);

            // Move card to discard or exile and disable visual
            playedCard.ClearVisual();

            if (playedCard.uses > 0) {
                playedCard.uses--;
            }

            if (playedCard.uses == 0) {
                exile.AddCard(playedCard);
            }
            else {
                discard.AddCard(playedCard);
            }

            // Push all DynamicEffects related to the card to the DynamicEffectController queue
            DynamicEffectController.Instance.AddEffects(playedCard.effects);

            // Update Elements played this turn
            ElementController.Instance.AddTurnElements(playedCard.GetOrderedElements());

            // Update visuals on screen
            PlayerController.Instance.UpdateVisual();
            UpdateVisuals();

            // Fire card played event
            OnCardPlay?.Invoke(playedCard);
        }
        else {
            // If the card play was canceled while player was choosing effects, return the card to hand
            playedCard.ReturnToHand();
            UpdateVisuals();
        }
    }
    #endregion

    #region DrawCard
    public void DrawCard() {
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
            OnCardDraw?.Invoke(drawnCard);
        }
    }
    #endregion

    #region DiscardCard
    public void DiscardRandomCard() {
        // When no card or index is given, a card is discarded at random
        // TODO
        UpdateVisuals();

        // Fire card played event
        // OnDiscard?.Invoke(discardedCard);
    }

    public void DiscardCard(int cardId) {
        // TODO
        UpdateVisuals();

        // Fire card played event
        // OnDiscard?.Invoke(discardedCard);
    }

    public void DiscardHand() {
        // This method is usually called at the end of the turn, but could be used in other circumstances
        List<Card> discarded = hand.GetCards();
        for (int i = 0; i < discarded.Count; i++) {
            Card card = discarded[i];
            if (card != null) {
                OnDiscard?.Invoke(card);
                card.ClearVisual();
                discard.AddCard(card);
            }
        }
        hand.ClearHand();
        UpdateVisuals();
    }
    #endregion

    public void ReturnDiscardToDeck() {
        deck.AddCards(discard.GetCards());
        discard.RemoveAll();
        deck.Shuffle();
    }

    // Returns a played card visual to the hand if it is cancelled or can't be played
    public void ReturnCardToHand(int cardId) {
        hand.GetCard(cardId).ReturnToHand();
        UpdateVisuals();
    }

    private void UpdateVisuals() {
        // Updates any visuals that display hand, deck, or discard values
        deckCount.text = deck.GetSize().ToString();
        discardCount.text = discard.GetSize().ToString();

        handArea.UpdateCardPositions();
    }
}
