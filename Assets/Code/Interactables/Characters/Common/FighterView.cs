using TMPro;
using UnityEngine;

public abstract class FighterView : BaseView {
    public readonly Fighter.FighterType fighterType;

    protected AttackView attackView;

    protected RectTransform healthBar;
    protected TextMeshProUGUI maxLife;
    protected TextMeshProUGUI lifeValue;

    protected SpriteRenderer sprite;

    protected int healthBarSize;

    public FighterView(GameObject visual, Fighter fighter, Fighter.FighterType fighterType, int healthBarSize) : base(visual, fighter.Id) {
        this.fighterType = fighterType;
        attackView = visual.transform.GetChild(1).GetComponent<AttackView>();
        attackView.InitializeView(fighter.Id, fighter.AttackValue, fighter.AttackTimes);
        this.healthBarSize = healthBarSize;
    }

    public RectTransform getVisualRect() {
        return visual.GetComponent<RectTransform>();
    }

    public void SetVisualOutline(Color color) {
        sprite.material.SetColor("_OutlineColor", color);
    }

    public void SetMaxLife(int val) {
        // NumberAnimator.Instance.AnimateNumberChange(this.maxLife, val);
        this.maxLife.text = val.ToString();
    }

    public void SetLife(bool hasLife, int life, int maxLife) {
        if (hasLife) {
            healthBar.transform.parent.gameObject.SetActive(true);
            // NumberAnimator.Instance.AnimateNumberChange(this.lifeValue, life);
            this.lifeValue.text = life.ToString();
            // Animate the healthbar
            float newSize = healthBarSize * ((float)life / (float)maxLife);
            LeanTween.size(healthBar, new Vector2(newSize, healthBar.sizeDelta.y), 0.2f);
        }
        else {
            healthBar.transform.parent.gameObject.SetActive(false);
        }
    }

    public void SetActive(bool active = true) {
        visual.SetActive(active);
    }

    public void Despawn() {
        ObjectPooler.Despawn(visual);
    }

    public void AnimateAttack() {
        AttackAnimator.Instance.AnimateAttack(sprite.gameObject, fighterType);
    }
}