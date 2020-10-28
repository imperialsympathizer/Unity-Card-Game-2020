using System;
using UnityEngine;

[Serializable]
public abstract class BaseInteractable {
    public int Id { get { return id; } }

    public readonly string name;
    private int id;
    protected readonly string description;

    public static event Action<int, string> OnInteractableNameChange;
    public static event Action<int, string> OnInteractableDescriptionChange;

    public static void ClearSubscriptions() {
        OnInteractableNameChange = null;
        OnInteractableDescriptionChange = null;
    }

    public BaseInteractable(string name, string description) {
        this.id = ResourceController.GenerateId();
        this.name = name;
        this.description = description;
    }

    public BaseInteractable(string name, string description, int id) {
        this.id = id;
        this.name = name;
        this.description = description;
    }

    public void ModifyId(int newId) {
        this.id = newId;
    }

    public abstract void CreateVisual();

    protected abstract void UpdateVisual();

    public abstract void EnableVisual(bool enable);

    public abstract void ClearVisual();

    public abstract RectTransform GetVisualRect();

    public abstract void SetVisualOutline(Color color);

    public abstract void SetVisualScale(Vector3 scale);
}
