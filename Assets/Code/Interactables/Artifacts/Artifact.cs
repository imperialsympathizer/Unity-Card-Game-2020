using System.Collections.Generic;
using UnityEngine;

public class Artifact : BaseInteractable {
    public readonly Rarity rarity;

    private List<Passive> effects;

    public Artifact(string name, string description, Rarity rarity, List<Passive> effects) : base(name, description) {
        this.rarity = rarity;
        this.effects = effects;
    }

    public override void ClearVisual() {
        throw new System.NotImplementedException();
    }

    public override void CreateVisual() {
        throw new System.NotImplementedException();
    }

    public override void EnableVisual(bool enable) {
        throw new System.NotImplementedException();
    }

    public override RectTransform GetVisualRect() {
        throw new System.NotImplementedException();
    }

    public override void SetVisualOutline(Color color) { }

    public override void UpdateVisual() {
        throw new System.NotImplementedException();
    }
}
