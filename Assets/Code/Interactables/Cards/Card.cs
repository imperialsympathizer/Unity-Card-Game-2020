using System;
using System.Collections.Generic;
using UnityEngine;

public class Card {
    private readonly string name;
    public readonly int id;
    private readonly string description;
    public int lifeCost { get; private set; }
    // This is the list of game effects to perform when a card is played
    // Needs to be publicly accessible to update targets
    public List<DynamicEffect> Effects;

    // Visual component of the card, stored within its own View class
    private CardView display;
    private GameObject prefab;

    // Constructor that creates the object, but does not instantiate visuals.
    // Those can be called as needed by the CreateVisual() function
    public Card(string name, int id, string description, int cost, List<DynamicEffect> effects) {
        this.name = name;
        this.id = id;
        this.description = description;
        this.lifeCost = cost;
        this.Effects = effects;
    }

    // Creates a Card with a new id (for copying cards and such)
    public Card(Card cardSource, int id) {
        this.name = cardSource.name;
        this.id = id;
        this.description = cardSource.description;
        this.lifeCost = cardSource.lifeCost;
        this.Effects = cardSource.Effects;
    }

    public void CreateVisual() {
        // Spawn an object to view the card on screen
        prefab = VisualController.SharedInstance.GetPrefab("CardPrefab");
        display = ObjectPooler.Spawn(prefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<CardView>();
        display.InitializeView(id, prefab);
        UpdateVisual();
    }

    public void UpdateVisual() {
        display.SetActive(false);
        display.SetName(name);
        display.SetCost(lifeCost);
        display.SetDescription(description);
        display.SetActive(true);
    }

    public void EnableVisual(bool enable) {
        display.SetActive(enable);
    }

    public RectTransform GetVisualRect() {
        return display.GetVisualRect();
    }

    public void SetVisualOutline(Color color) {
        display.SetVisualOutline(color);
    }

    // Function to call before moving card off the screen to another location (such as deck or discard)
    public void ClearVisual() {
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
