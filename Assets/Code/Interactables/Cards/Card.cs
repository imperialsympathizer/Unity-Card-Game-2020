using System.Collections.Generic;
using UnityEngine;

public class Card : BaseInteractable {
    public int LifeCost { get; private set; }

    public readonly CardRarity rarity;
    // This is the list of game effects to perform when a card is played
    // Needs to be publicly accessible to update targets
    public List<DynamicEffect> effects;

    // Visual component of the card, stored within its own View class
    private CardView display;

    public enum CardRarity {
        COMMON,
        UNCOMMON,
        RARE,
        VERY_RARE
    }

    // Constructor that creates the object, but does not instantiate visuals.
    // Those can be called as needed by the CreateVisual() function
    public Card(string name, string description, int cost, CardRarity rarity, List<DynamicEffect> effects) : base(name, description) {
        this.LifeCost = cost;
        this.rarity = rarity;
        this.effects = effects;
    }

    // Creates a Card with a new id (for copying cards and such)
    public Card(Card cardSource) : base(cardSource.name, cardSource.description) {
        this.LifeCost = cardSource.LifeCost;
        this.effects = cardSource.effects;
    }

    public void UpdateLifeCost(int val) {
        LifeCost += val;
        if (LifeCost < 0) {
            LifeCost = 0;
        }
        UpdateVisual();
    }

    public override void CreateVisual() {
        // Spawn an object to view the card on screen
        display = new CardView(ObjectPooler.Spawn(VisualController.SharedInstance.GetPrefab("CardPrefab"), new Vector3(0, 0, 0), Quaternion.identity), id);
        UpdateVisual();
    }

    public override void UpdateVisual() {
        display.SetActive(false);
        display.SetName(name);
        display.SetCost(LifeCost);
        display.SetDescription(description);
        display.SetActive(true);
    }

    public override void EnableVisual(bool enable) {
        display.SetActive(enable);
    }

    public override RectTransform GetVisualRect() {
        return display.GetVisualRect();
    }

    public override void SetVisualOutline(Color color) {
        display.SetVisualOutline(color);
    }

    // Function to call before moving card off the screen to another location (such as deck or discard)
    public override void ClearVisual() {
        if (display != null) {
            // TODO: add a dissolve animation before despawning
            display.Despawn();
            display = null;
        }
    }

    public void ReturnToHand() {
        UpdateVisual();
        display.MoveToHand();
    }
}
