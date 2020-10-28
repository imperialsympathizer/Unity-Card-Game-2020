using UnityEngine;

public abstract class BaseView {
    protected GameObject visual;

    public int Id { get { return id; } }

    private int id;

    public BaseView(GameObject visual, int id) {
        this.visual = visual;
        this.id = id;
    }

    public void ModifyId(int newId) {
        this.id = newId;
    }

    public void SetVisualScale(Vector3 scale) {
        visual.transform.localScale = scale;
    }
}