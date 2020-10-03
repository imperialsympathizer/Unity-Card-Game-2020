using TMPro;
using UnityEngine;

public class EnemyView : FighterView {
    public EnemyView(GameObject view, int id, int healthBarSize) : base(view, id, Fighter.FighterType.ENEMY, healthBarSize) {
        visual.SetActive(false);
        VisualController.SharedInstance.ParentToEnemyCanvas(visual.transform);
        visual.transform.localScale = new Vector3(1, 1, 1);

        sprite = visual.transform.GetChild(0).GetComponent<SpriteRenderer>();
        attackValue = visual.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        attackTimes = visual.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        xText = visual.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        healthBar = visual.transform.GetChild(4).GetChild(1).GetComponent<RectTransform>();
        maxLife = visual.transform.GetChild(4).GetChild(3).GetComponent<TextMeshProUGUI>();
        lifeValue = visual.transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>();
        visual.SetActive(true);
    }
}
