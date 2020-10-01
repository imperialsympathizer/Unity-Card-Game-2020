using TMPro;
using UnityEngine;

public class SummonView {
    private GameObject visual;

    private int id;

    private TextMeshProUGUI attackValue;
    private TextMeshProUGUI xText;
    private TextMeshProUGUI attackTimes;

    private RectTransform healthBar;
    private TextMeshProUGUI maxLife;
    private TextMeshProUGUI lifeValue;

    private SpriteRenderer sprite;

    public void InitializeView(GameObject summon, int id) {
        this.id = id;
        visual = summon;
        visual.SetActive(false);
        VisualController.SharedInstance.ParentToSummonCanvas(visual.transform);
        visual.transform.localScale = new Vector3(1, 1, 1);
        visual.transform.localPosition = new Vector3(0, 0, -10);

        sprite = visual.transform.GetChild(0).GetComponent<SpriteRenderer>();
        attackValue = visual.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        attackTimes = visual.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        xText = visual.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        healthBar = visual.transform.GetChild(4).GetChild(1).GetComponent<RectTransform>();
        maxLife = visual.transform.GetChild(4).GetChild(3).GetComponent<TextMeshProUGUI>();
        lifeValue = visual.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>();
        visual.SetActive(true);
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
            float healthBarSize = 410 * ((float)life / (float)maxLife);
            LeanTween.size(healthBar, new Vector2(healthBarSize, healthBar.sizeDelta.y), 0.2f);
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
        AttackAnimator.AnimateAttack(visual, AttackAnimator.AttackerType.SUMMON);
    }
}
