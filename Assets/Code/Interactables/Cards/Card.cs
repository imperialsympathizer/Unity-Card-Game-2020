using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card {
    private string name;
    public int Id { get; private set; }
    private string description;
    public int lifeCost { get; private set; }
    public bool hasTargets { get; private set; }
    // this is the list of game effects to perform when a card is played
    public List<PlayEffect> Effects { get; private set; }

    // Visual component of the card, stored within its own View class
    private CardView display;
    private GameObject prefab;

    // Constructor that creates the object, but does not instantiate visuals.
    // Those can be called as needed by the CreateVisual() function
    public Card(string name, int id, string description, int cost, bool hasTargets, List<PlayEffect> effects) {
        this.name = name;
        this.Id = id;
        this.description = description;
        this.lifeCost = cost;
        this.hasTargets = hasTargets;
        this.Effects = effects;
    }

    // Creates a Card with a new id (for copying cards and such)
    public Card(Card cardSource, int id) {
        this.name = cardSource.name;
        this.Id = id;
        this.description = cardSource.description;
        this.lifeCost = cardSource.lifeCost;
        this.hasTargets = cardSource.hasTargets;
        this.Effects = cardSource.Effects;
    }

    public void CreateVisual() {
        // Spawn an object to view the card on screen
        prefab = VisualController.SharedInstance.GetPrefab("CardPrefab");
        display = ObjectPooler.Spawn(prefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<CardView>();
        display.InitializeView(Id, prefab);
        display.SetActive(false);
        display.SetName(name);
        display.SetCost(lifeCost);
        display.SetDescription(description);
        display.SetActive(true);
    }

    public void DisableVisual() {
        display.SetActive(false);
    }

    // Function to call before moving card off the screen to another location (such as deck or discard)
    public void ClearVisual() {
        if (display != null) {
            display.Despawn();
            display = null;
        }
    }

    // When a card is played, for each PlayEffect that requires targets, request the player to pick them
    // Returns whether all targets were chosen (if there were no targets, will return true)
    public bool SetTargets() {
        bool targetsChosen = true;
        for (int i = 0; i < Effects.Count; i++) {
            PlayEffect effect = Effects[i];
            // if GetValidTargets returns null, the effect does not target
            if (effect is TargetablePlayEffect) {
                // Set the card visual out of the way while targets are chosen
                // TargetSelector will enable the targeting canvas and make targetable objects selectable
                DisableVisual();
                TargetSelector.SharedInstance.EnableTargetCanvas();

                // If this is the first effect

                // Should make a call to TargetSelector for target selection
                targetsChosen = false;
            }
        }

        return targetsChosen;
    }
}
