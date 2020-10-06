using System.Collections.Generic;
using UnityEngine;

public class Card : BaseInteractable {
    public int lifeCost { get; private set; }
    // This is the list of game effects to perform when a card is played
    // Needs to be publicly accessible to update targets
    public List<DynamicEffect> Effects;

    // Visual component of the card, stored within its own View class
    private CardView display;

    // Constructor that creates the object, but does not instantiate visuals.
    // Those can be called as needed by the CreateVisual() function
    public Card(string name, string description, int cost, List<DynamicEffect> effects) : base(name, description) {
        this.lifeCost = cost;
        this.Effects = effects;
    }

    // Creates a Card with a new id (for copying cards and such)
    public Card(Card cardSource) : base(cardSource.name, cardSource.description) {
        this.lifeCost = cardSource.lifeCost;
        this.Effects = cardSource.Effects;
    }

    public override void CreateVisual() {
        // Spawn an object to view the card on screen
        display = new CardView(ObjectPooler.Spawn(VisualController.SharedInstance.GetPrefab("CardPrefab"), new Vector3(0, 0, 0), Quaternion.identity), id);
        UpdateVisual();
    }

    public override void UpdateVisual() {
        display.SetActive(false);
        display.SetName(name);
        display.SetCost(lifeCost);
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
