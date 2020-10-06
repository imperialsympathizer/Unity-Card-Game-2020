using UnityEngine;

public abstract class BaseInteractable {
    public readonly string name;
    public readonly int id;
    protected readonly string description;

    public BaseInteractable(string name, string description) {
        this.id = ResourceController.GenerateId();
        this.name = name;
        this.description = description;
    }

    public abstract void CreateVisual();

    public abstract void UpdateVisual();

    public abstract void EnableVisual(bool enable);

    public abstract void ClearVisual();

    public abstract RectTransform GetVisualRect();

    public abstract void SetVisualOutline(Color color);
}
