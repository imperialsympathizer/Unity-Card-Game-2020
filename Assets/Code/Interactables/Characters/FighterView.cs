using TMPro;
using UnityEngine;

public abstract class FighterView {
    public readonly Fighter.FighterType fighterType;

    protected GameObject visual;

    protected int id;

    protected TextMeshProUGUI attackValue;
    protected TextMeshProUGUI xText;
    protected TextMeshProUGUI attackTimes;

    protected RectTransform healthBar;
    protected TextMeshProUGUI maxLife;
    protected TextMeshProUGUI lifeValue;

    protected SpriteRenderer sprite;

    protected int healthBarSize;

    public FighterView(GameObject view, int id, Fighter.FighterType fighterType, int healthBarSize) {
        this.visual = view;
        this.id = id;
        this.fighterType = fighterType;
        this.healthBarSize = healthBarSize;
    }

    public void SetAttack(int val) {
        NumberAnimator.AnimateNumberChange(this.attackValue, val);
    }

    public void SetMaxLife(int val) {
        NumberAnimator.AnimateNumberChange(this.maxLife, val);
    }

    public void SetLife(bool hasLife, int life, int maxLife) {
        if (hasLife) {
            healthBar.transform.parent.gameObject.SetActive(true);
            NumberAnimator.AnimateNumberChange(this.lifeValue, life);
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
        AttackAnimator.AnimateAttack(sprite.gameObject, fighterType);
    }
}