using UnityEngine;

public abstract class BaseView {
    protected GameObject visual;

    protected int id;

    public BaseView(GameObject visual, int id) {
        this.visual = visual;
        this.id = id;
    }
}