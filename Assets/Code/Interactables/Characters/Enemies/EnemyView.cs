using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView {
    private GameObject visual;

    private int id;

    private TextMeshProUGUI attackValue;
    private TextMeshProUGUI xText;
    private TextMeshProUGUI attackTimes;
    private TextMeshProUGUI maxLife;
    private TextMeshProUGUI lifeValue;
    private RectTransform healthBar;

    private SpriteRenderer sprite;

    public void InitializeView(GameObject enemy, int id) {
        this.id = id;
        visual = enemy;
        visual.SetActive(false);
        sprite = visual.transform.GetChild(0).GetComponent<SpriteRenderer>();
        VisualController.SharedInstance.ParentToEnemyCanvas(visual.transform);
        // When parented to the enemy canvas, for some reason the sprite likes to size itself to an abritrarily large amount
        // This ensures its scale is correct
        visual.transform.localScale = new Vector3(1, 1, 1);
        visual.transform.localPosition = new Vector3(0, 0, -50);
        // Set outline alpha of sprite to 0
        sprite.material.SetColor("_OutlineColor", new Color(0, 0, 0, 0));

        attackValue = visual.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        attackTimes = visual.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        xText = visual.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        maxLife = visual.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>();
        lifeValue = visual.transform.GetChild(4).GetChild(3).GetComponent<TextMeshProUGUI>();
        healthBar = visual.transform.GetChild(4).GetChild(1).GetComponent<RectTransform>();
        visual.SetActive(true);
    }

    public void SetAttack(int val) {
        NumberAnimator.SharedInstance.AnimateNumberChange(this.attackValue, val);
    }

    public void SetMaxLife(int val) {
        NumberAnimator.SharedInstance.AnimateNumberChange(this.maxLife, val);
    }

    public void SetLife(int life, int maxLife) {
        NumberAnimator.SharedInstance.AnimateNumberChange(this.lifeValue, life);
        // Animate the healthbar
        float healthBarSize = 410 * ((float)life / (float)maxLife);
        LeanTween.size(healthBar, new Vector2(healthBarSize, healthBar.sizeDelta.y), 0.2f);
    }

    public void SetAttackTimes(int val) {
        // If the val is 1, disable the x and AttackTimes displays
        // If val is 0, disable all attack displays
        if (val == 1) {
            attackValue.gameObject.SetActive(false);
            attackTimes.gameObject.SetActive(false);
            xText.text = attackValue.text;
            // Use the xText display to display attack since it is the box centered over the character
            // it will be used as the attackValue display unless a character has attackTimes > 1
        }
        else if (val == 0) {
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
        this.attackTimes.text = val.ToString();
    }

    public void SetActive(bool active = true) {
        visual.SetActive(active);
    }

    public void Despawn() {
        ObjectPooler.Despawn(visual);
    }
}
