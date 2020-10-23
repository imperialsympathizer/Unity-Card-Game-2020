using System;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : BaseInteractable {
    public readonly Rarity rarity;

    // Keyed by element, first tuple value is amount required, second tuple value is current stored amount
    public Dictionary<Element.ElementType, Tuple<int, int>> elementsRequired;
    public List<Element.ElementType> typesRequired;
    // Some Artifacts trigger on TurnElements, some are passive effects dependent on TotalElements
    public readonly bool controlledByTurnElements;

    private List<ArtifactPassive> effects;
    private ArtifactView display;

    public static event Action<int> OnArtifactActivate;

    // This constructor is to be exclusively used for creating Artifact data, not Artifact objects
    // If an Artifact object needs to be created, use the copy constructor
    public Artifact(string name,
        string description,
        Rarity rarity,
        List<ArtifactPassive> effects,
        List<Element> elementsRequired,
        bool controlledByTurnElements)
        : base(name, description) {
        this.rarity = rarity;
        this.effects = effects;
        this.controlledByTurnElements = controlledByTurnElements;

        // Initialize required elements
        this.typesRequired = new List<Element.ElementType>();
        this.elementsRequired = new Dictionary<Element.ElementType, Tuple<int, int>>();
        foreach (Element element in elementsRequired) {
            if (this.elementsRequired.TryGetValue(element.type, out Tuple<int, int> elementValues)) {
                this.elementsRequired[element.type] = new Tuple<int, int>(elementValues.Item1 + element.count, 0);
            }
            else {
                this.typesRequired.Add(element.type);
                this.elementsRequired.Add(element.type, new Tuple<int, int>(element.count, 0));
            }
        }
    }

    public Artifact(Artifact artifactToCopy) : base(artifactToCopy.name, artifactToCopy.description) {
        this.rarity = artifactToCopy.rarity;
        this.effects = new List<ArtifactPassive>(artifactToCopy.effects);
        this.typesRequired = new List<Element.ElementType>(artifactToCopy.typesRequired);
        this.elementsRequired = new Dictionary<Element.ElementType, Tuple<int, int>>(artifactToCopy.elementsRequired);
        this.controlledByTurnElements = artifactToCopy.controlledByTurnElements;

        // Initialize all ArtifactPassives with the correct artifactId
        foreach (ArtifactPassive effect in this.effects) {
            effect.artifactId = id;
            StaticEffectController.Instance.AddPassive(effect);
        }
    }

    public override void ClearVisual() {
        throw new System.NotImplementedException();
    }

    public override void CreateVisual() {
        // Spawn an object to view the card on screen
        display = new ArtifactView(ObjectPooler.Spawn(VisualController.Instance.GetPrefab("Artifact Icon"), new Vector3(0, 0, 0), Quaternion.identity), this);
        if (controlledByTurnElements) {
            ElementController.OnTurnElementUpdate += OnTurnElementUpdate;
        }
        else {
            ElementController.OnTotalElementUpdate += OnTotalElementUpdate;
        }
        UpdateVisual();
    }

    public override void EnableVisual(bool enable) {
        // TODO
    }

    public override RectTransform GetVisualRect() {
        // TODO
        return null;
    }

    public override void SetVisualOutline(Color color) {
        // TODO
    }

    public override void UpdateVisual() {
        display.HideTooltip();
        display.UpdateElementCounts(elementsRequired);
        display.DisplayTooltip();
    }

    private void OnTotalElementUpdate(Dictionary<Element.ElementType, int> elementUpdate) {
        throw new NotImplementedException();
    }

    private void OnTurnElementUpdate(List<Element> elementUpdate) {
        // add each element (if relevant) to the current element tally
        foreach (Element element in elementUpdate) {
            if (elementsRequired.TryGetValue(element.type, out Tuple<int, int> elementValues)) {
                elementsRequired[element.type] = new Tuple<int, int>(elementValues.Item1, elementValues.Item2 + element.count);
            }
        }

        // Trigger for as many times as the threshold is met
        while (CheckThresholdMet()) {
            // Send trigger to all effects controlled by this artifact
            OnArtifactActivate?.Invoke(id);
            ReduceCountsByThreshold();
        }

        UpdateVisual();
    }

    private bool CheckThresholdMet() {
        bool thresholdMet = true;
        foreach (Element.ElementType elementRequired in typesRequired) {
            if (elementsRequired.TryGetValue(elementRequired, out Tuple<int, int> values)) {
                if (values.Item1 > values.Item2) {
                    thresholdMet = false;
                    break;
                }
            }
            else {
                throw new Exception("Artifact does not have any effect values correlating to a required element type.");
            }
        }

        return thresholdMet;
    }

    private void ReduceCountsByThreshold() {
        foreach (Element.ElementType elementRequired in typesRequired) {
            if (elementsRequired.TryGetValue(elementRequired, out Tuple<int, int> values)) {
                elementsRequired[elementRequired] = new Tuple<int, int>(values.Item1, values.Item2 - values.Item1);
            }
            else {
                throw new Exception("Artifact does not have any effect values correlating to a required element type.");
            }
        }
    }
}
