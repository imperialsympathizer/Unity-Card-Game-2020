using System;
using System.Collections.Generic;
using UnityEngine;

public class Card : BaseInteractable {
    public int LifeCost { get { return lifeCost; } }

    private int lifeCost;

    public readonly Rarity rarity;
    // This is the list of game effects to perform when a card is played
    // Needs to be publicly accessible to update targets
    public List<DynamicEffect> effects;

    // Number of times the card can be played before being exiled
    public int uses;

    private Dictionary<Element.ElementType, Element> elements;
    public int ElementTotal { get; private set; }

    // Visual component of the card, stored within its own View class
    private CardView display;

    public static event Action<int, int> OnCardCostChange;
    public static event Action<int, List<Element>> OnElementsChange;

    public static new void ClearSubscriptions() {
        OnCardCostChange = null;
        OnElementsChange = null;
    }

    // Constructor that creates the object, but does not instantiate visuals.
    // Those can be called as needed by the CreateVisual() function
    public Card(string name, string description, int cost, Rarity rarity, int uses, List<DynamicEffect> effects, List<Element> elements) : base(name, description) {
        this.lifeCost = cost;
        this.rarity = rarity;
        this.effects = new List<DynamicEffect>();
        foreach (DynamicEffect effect in effects) {
            effect.ModifyId(ResourceController.GenerateId());
            this.effects.Add(effect);
        }
        this.uses = uses;
        this.ElementTotal = 0;
        this.elements = new Dictionary<Element.ElementType, Element>();
        foreach (Element element in elements) {
            UpdateElements(element.type, element.count);
        }
    }

    // Creates a Card with a new Id (for copying cards and such)
    public Card(Card cardSource) : base(cardSource.name, cardSource.description) {
        this.lifeCost = cardSource.LifeCost;
        this.effects = new List<DynamicEffect>();
        foreach (DynamicEffect effect in cardSource.effects) {
            effect.ModifyId(ResourceController.GenerateId());
            this.effects.Add(effect);
        }
        this.uses = cardSource.uses;
        this.ElementTotal = cardSource.ElementTotal;
        this.elements = new Dictionary<Element.ElementType, Element>(cardSource.elements);
    }

    // Creates a Card with the same Id
    // Used for separation of run deck from battle deck, but allows linking between the cards later through Id
    public Card(Card cardSource, bool copyId) : base(cardSource.name, cardSource.description, copyId? cardSource.Id : -1) {
        this.lifeCost = cardSource.LifeCost;
        this.effects = new List<DynamicEffect>();
        foreach (DynamicEffect effect in cardSource.effects) {
            effect.ModifyId(ResourceController.GenerateId());
            this.effects.Add(effect);
        }
        this.uses = cardSource.uses;
        this.ElementTotal = cardSource.ElementTotal;
        this.elements = new Dictionary<Element.ElementType, Element>(cardSource.elements);
    }

    public void UpdateLifeCost(int val) {
        lifeCost += val;
        if (lifeCost < 0) {
            lifeCost = 0;
        }
        OnCardCostChange?.Invoke(Id, lifeCost);
    }

    public void UpdateElements(Element.ElementType type, int count) {
        UpdateElements(new Element(type, count));
    }

    public void UpdateElements(Element element) {
        // Cannot have more than 7 total elements on a card
        if (ElementTotal + element.count > 7) {
            element.count = 7 - ElementTotal;
        }
        if (element.count != 0) {
            // If element already exists in dictionary, update the count
            // Otherwise, create a new entry for that element
            if (elements.TryGetValue(element.type, out Element updateElement)) {
                // replace null values
                if (updateElement == null) {
                    elements[element.type] = element;
                    ElementTotal += element.count;
                }
                else {
                    // if resulting element count is 0 or less, remove element from dictionary
                    updateElement.count += element.count;
                    if (updateElement.count < 1) {
                        elements.Remove(updateElement.type);
                        ElementTotal -= element.count + updateElement.count;
                    }
                    else {
                        ElementTotal += element.count;
                    }
                }
            }
            else if (element.count > 0) {
                elements.Add(element.type, element);
                ElementTotal += element.count;
            }
        }
        OnElementsChange?.Invoke(Id, GetOrderedElements());
    }

    public List<Element> GetOrderedElements() {
        // Returns an ordered list of elements
        List<Element> elementList = new List<Element>();
        Element element;
        if (elements.TryGetValue(Element.ElementType.AIR, out element) && element != null) {
            elementList.Add(element);
        }
        if (elements.TryGetValue(Element.ElementType.EARTH, out element) && element != null) {
            elementList.Add(element);
        }
        if (elements.TryGetValue(Element.ElementType.FIRE, out element) && element != null) {
            elementList.Add(element);
        }
        if (elements.TryGetValue(Element.ElementType.WATER, out element) && element != null) {
            elementList.Add(element);
        }
        if (elements.TryGetValue(Element.ElementType.LIFE, out element) && element != null) {
            elementList.Add(element);
        }
        if (elements.TryGetValue(Element.ElementType.DEATH, out element) && element != null) {
            elementList.Add(element);
        }
        if (elements.TryGetValue(Element.ElementType.ARTIFICE, out element) && element != null) {
            elementList.Add(element);
        }

        return elementList;
    }

    public override void CreateVisual() {
        // Spawn an object to view the card on screen
        display = new CardView(ObjectPooler.Spawn(VisualController.Instance.GetPrefab("CardPrefab"), new Vector3(0, 0, 0), Quaternion.identity), Id);
        UpdateVisual();
    }

    // Creates a card that cannot be picked up/controlled and parents it to the given transform
    public void CreateDudVisual(Transform parent, float scale) {
        // Spawn an object to view the card on screen
        display = new CardView(ObjectPooler.Spawn(VisualController.Instance.GetPrefab("CardDudPrefab"), new Vector3(0, 0, 0), Quaternion.identity), Id, parent, scale);
        UpdateVisual();
    }

    public override void UpdateVisual() {
        display.SetActive(false);
        display.SetName(Id, name);
        display.SetCost(Id, lifeCost);
        display.SetDescription(Id, description);
        display.SetElements(Id, GetOrderedElements());
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

    public override void SetVisualScale(Vector3 scale) {
        display.SetVisualScale(scale);
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
