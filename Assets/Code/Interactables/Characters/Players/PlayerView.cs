using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerView : FighterView {
    private TextMeshProUGUI willValue;

    // Visual component of the slots
    private List<GameObject> slots = new List<GameObject>();
    private GameObject slotPrefab;

    public PlayerView(GameObject player, int id, GameObject slotPrefab, int startSlots) : base(player, id, Fighter.FighterType.PLAYER, 0) {
        this.slotPrefab = slotPrefab;
        visual.SetActive(false);
        sprite = visual.transform.GetChild(0).GetComponent<SpriteRenderer>();
        VisualController.SharedInstance.ParentToPlayerCanvas(visual.transform);
        visual.transform.localScale = new Vector3(1, 1, 1);
        visual.transform.localPosition = new Vector3(visual.transform.localPosition.x, visual.transform.localPosition.y, -20);

        attackValue = visual.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        attackTimes = visual.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        xText = visual.transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        lifeValue = VisualController.SharedInstance.GetLifeValue().GetComponent<TextMeshProUGUI>();
        willValue = VisualController.SharedInstance.GetWillValue().GetComponent<TextMeshProUGUI>();
        visual.SetActive(true);

        // Spawn a slot for each starting slot the player has
        for (int i = 0; i < startSlots; i++) {
            AddSlot();
        }
    }

    public new void SetLife(bool nothing, int life, int maxLife) {
        NumberAnimator.AnimateNumberChange(this.lifeValue, life);
    }

    public void SetWill(int val) {
        NumberAnimator.AnimateNumberChange(this.willValue, val);
    }

    public new void SetActive(bool active = true) {
        visual.SetActive(active);
        lifeValue.gameObject.SetActive(active);
        willValue.gameObject.SetActive(active);
    }

    public new void Despawn() {}

    public void AddSlot() {
        GameObject newSlot = ObjectPooler.Spawn(slotPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newSlot.SetActive(false);
        VisualController.SharedInstance.ParentToSlotCanvas(newSlot.transform);
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
