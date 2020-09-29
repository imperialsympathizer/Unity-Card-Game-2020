using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SummonView {
    private GameObject visual;

    private int id;

    private TextMeshProUGUI attackValue;
    private TextMeshProUGUI xText;
    private TextMeshProUGUI attackTimes;
    private TextMeshProUGUI lifeValue;

    public void InitializeView(GameObject summon, int id) {
        this.id = id;
        visual = summon;
        visual.SetActive(false);
        VisualController.SharedInstance.ParentToSummonCanvas(visual.transform);
        // When parented to the player canvas, for some reason the sprite likes to size itself to an abritrarily large amount
        // This ensures its scale is correct
        visual.transform.localScale = new Vector3(1, 1, 1);
        visual.transform.localPosition = new Vector3(0, 0, -50);

        attackValue = visual.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        attackTimes = visual.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        xText = visual.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        lifeValue = visual.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        visual.SetActive(true);
    }

    public void SetAttack(int val) {
        this.attackValue.text = val.ToString();
    }

    public void SetLife(int val) {
        this.lifeValue.text = val.ToString();
    }

    public void SetAttackTimes(int val) {
        // If the val is 1, disable the x and AttackTimes displays
        // If val is 0, disable all attack displays
        if (val == 1) {
            attackValue.gameObject.SetActive(false);
            attackTimes.gameObject.SetActive(false);
            xText.text = attackValue.text;
            // Use the xtText display to display attack since it is the box centered over the character
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
