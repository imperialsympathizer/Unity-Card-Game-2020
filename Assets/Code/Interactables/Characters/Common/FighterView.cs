using TMPro;
using UnityEngine;

public abstract class FighterView : BaseView {
    public readonly Fighter.FighterType fighterType;

    protected TextMeshProUGUI attackValue;
    protected TextMeshProUGUI xText;
    protected TextMeshProUGUI attackTimes;

    protected RectTransform healthBar;
    protected TextMeshProUGUI maxLife;
    protected TextMeshProUGUI lifeValue;

    protected SpriteRenderer sprite;

    protected int healthBarSize;

    public FighterView(GameObject visual, int id, Fighter.FighterType fighterType, int healthBarSize) : base(visual, id) {
        this.fighterType = fighterType;
        this.healthBarSize = healthBarSize;
    }

    public RectTransform getVisualRect() {
        return visual.GetComponent<RectTransform>();
    }

    public void SetVisualOutline(Color color) {
        sprite.material.SetColor("_OutlineColor", color);
    }

    public void SetAttack(int val) {
        // NumberAnimator.Instance.AnimateNumberChange(this.attackValue, val);
        this.attackValue.text = val.ToString();
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

    public void SetAttackTimes(int attackVal, int times) {
        // If the val is 1, disable the x and AttackTimes displays
        // If val is 0, disable all attack displays
        if (times == 1) {
            attackValue.gameObject.SetActive(false);
            attackTimes.gameObject.SetActive(false);
            xText.text = attackVal.ToString();
            // Use the xText display to display attack since it is the box centered over the character
            // it will be used as the attackValue display unless a character has attackTimes > 1
        }
        else if (times == 0) {
            attackValue.gameObject.SetActive(false);
            attackTimes.gameObject.SetActive(false);
            xText.gameObject.SetActive(false);
        }
        else {
            // Make sure to reset the xText
            xText.text = "x";
            attackValue.gameObject.SetActive(true);
            attackTimes.gameObject.SetActive(true);
            xText.gameObject.SetActive(true);
        }
        this.attackTimes.text = times.ToString();
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