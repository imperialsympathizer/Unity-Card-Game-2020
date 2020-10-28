using TMPro;
using UnityEngine;

public class SummonView : FighterView {
    public SummonView(GameObject view, Summon summon, int healthBarSize) : base(view, summon, Fighter.FighterType.SUMMON, healthBarSize) {
        visual.SetActive(false);
        VisualController.Instance.ParentToSummonCanvas(visual.transform);
        visual.transform.localScale = new Vector3(1, 1, 1);
        visual.transform.localPosition = new Vector3(0, 0, -10);

        sprite = visual.transform.GetChild(0).GetComponent<SpriteRenderer>();
        //attackValue = visual.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        //attackTimes = visual.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        //xText = visual.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        healthBar = visual.transform.GetChild(2).GetChild(1).GetComponent<RectTransform>();
        maxLife = visual.transform.GetChild(2).GetChild(3).GetComponent<TextMeshProUGUI>();
        lifeValue = visual.transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();
        visual.SetActive(true);
    }
}
