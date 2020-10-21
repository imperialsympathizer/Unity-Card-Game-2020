using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerView : FighterView {
    private TextMeshProUGUI willValue;
    private TextMeshProUGUI vigorValue;

    protected RectTransform willIcon;
    protected RectTransform lifeIcon;
    protected RectTransform vigorIcon;

    private readonly int willIconSize = 280;
    private readonly int lifeIconSize = 290;
    private readonly int vigorIconSize = 270;

    // Visual component of the slots
    private List<GameObject> slots = new List<GameObject>();
    private GameObject slotPrefab;

    public PlayerView(GameObject player, int id, GameObject slotPrefab, int startSlots) : base(player, id, Fighter.FighterType.PLAYER, 0) {
        this.slotPrefab = slotPrefab;
        visual.SetActive(false);
        sprite = visual.transform.GetChild(0).GetComponent<SpriteRenderer>();
        VisualController.Instance.ParentToPlayerCanvas(visual.transform);
        visual.transform.localScale = new Vector3(1, 1, 1);
        visual.transform.localPosition = new Vector3(visual.transform.localPosition.x, visual.transform.localPosition.y, -20);

        attackValue = visual.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        attackTimes = visual.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        xText = visual.transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        willIcon = VisualController.Instance.GetWillIcon().transform.GetChild(1).GetComponent<RectTransform>();
        lifeIcon = VisualController.Instance.GetLifeIcon().transform.GetChild(1).GetComponent<RectTransform>();
        vigorIcon = VisualController.Instance.GetVigorIcon().transform.GetChild(1).GetComponent<RectTransform>();

        lifeValue = VisualController.Instance.GetLifeIcon().transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        willValue = VisualController.Instance.GetWillIcon().transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        vigorValue = VisualController.Instance.GetVigorIcon().transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        visual.SetActive(true);

        // Spawn a slot for each starting slot the player has
        for (int i = 0; i < startSlots; i++) {
            AddSlot();
        }
    }

    public new void SetLife(bool nothing, int life, int maxLife) {
        NumberAnimator.Instance.AnimateNumberChange(this.lifeValue, life);
        // Animate the icon
        float newSize = System.Math.Max(0, lifeIconSize * ((float)life / (float)maxLife));
        LeanTween.cancel(lifeIcon);
        // If the icon is "increasing" (by losing a will), animate it dropping to 0, then animate it dropping from full
        if (newSize > lifeIcon.sizeDelta.y) {
            LeanTween.size(lifeIcon, new Vector2(lifeIcon.sizeDelta.x, 0), 0.3f).setOnComplete(() => {
                lifeIcon.sizeDelta = new Vector2(lifeIcon.sizeDelta.x, lifeIconSize);
                LeanTween.size(lifeIcon, new Vector2(lifeIcon.sizeDelta.x, newSize), 0.3f);
            });
        }
        else {
            LeanTween.size(lifeIcon, new Vector2(lifeIcon.sizeDelta.x, newSize), 0.3f);
        }
    }

    public void SetWill(int val, int maxWill) {
        NumberAnimator.Instance.AnimateNumberChange(this.willValue, val);
        float newSize = willIconSize * ((float)val / (float)maxWill);
        LeanTween.cancel(willIcon);
        LeanTween.size(willIcon, new Vector2(willIcon.sizeDelta.x, newSize), 0.3f);
    }

    public void SetVigor(int val, int maxLife) {
        NumberAnimator.Instance.AnimateNumberChange(this.vigorValue, val);
        float newSize = vigorIconSize * ((float)val / (float)maxLife);
        LeanTween.cancel(vigorIcon);
        LeanTween.size(vigorIcon, new Vector2(vigorIcon.sizeDelta.x, newSize), 0.3f);
    }

    public new void SetActive(bool active = true) {
        visual.SetActive(active);
        lifeValue.gameObject.SetActive(active);
        willValue.gameObject.SetActive(active);
    }

    public new void Despawn() { }

    public void AddSlot() {
        GameObject newSlot = ObjectPooler.Spawn(slotPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newSlot.SetActive(false);
        VisualController.Instance.ParentToSlotCanvas(newSlot.transform);
        newSlot.transform.localScale = new Vector3(1, 1, 1);
        newSlot.transform.localPosition = new Vector3(0, 0, 0);
        newSlot.SetActive(true);
        slots.Add(newSlot);
    }

    public void RemoveSlot() {
        GameObject slot = slots[0];
        ObjectPooler.Despawn(slot);
        slots.RemoveAt(0);
    }
}
