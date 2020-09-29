using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerView  {
    private GameObject visual;

    private int id;

    private TextMeshProUGUI attackValue;
    private TextMeshProUGUI xText;
    private TextMeshProUGUI attackTimes;
    private TextMeshProUGUI lifeValue;
    private TextMeshProUGUI willValue;

    // Sprite Renderer of the player
    private SpriteRenderer sprite;

    // Visual component of the slots
    private List<GameObject> slots = new List<GameObject>();
    private GameObject slotPrefab;

    public void InitializeView(GameObject player, int id, GameObject slotPrefab, int startSlots) {
        this.id = id;
        visual = player;
        this.slotPrefab = slotPrefab;
        visual.SetActive(false);
        sprite = visual.transform.GetChild(0).GetComponent<SpriteRenderer>();
        VisualController.SharedInstance.ParentToPlayerCanvas(visual.transform);
        // When parented to the canvas, for some reason the sprite likes to size itself to an arbitrarily large amount
        // This ensures its scale is correct
        visual.transform.localScale = new Vector3(1, 1, 1);
        visual.transform.localPosition = new Vector3(0, 0, -100);
        // Set outline alpha of sprite to 0
        sprite.material.SetColor("_OutlineColor", new Color(0, 0, 0, 0));


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

    public void SetAttack(int val) {
        this.attackValue.text = val.ToString();
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

    public void SetLife(int val) {
        NumberAnimator.SharedInstance.AnimateNumberChange(this.lifeValue, val);
    }

    public void SetWill(int val) {
        NumberAnimator.SharedInstance.AnimateNumberChange(this.willValue, val);
    }

    public void SetActive(bool active = true) {
        visual.SetActive(active);
        lifeValue.gameObject.SetActive(active);
        willValue.gameObject.SetActive(active);
    }

    public void Despawn() {

    }

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
