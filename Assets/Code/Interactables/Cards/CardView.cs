using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour {
    private GameObject visual;

    private int Id;

    // TODO: add an object to the card that can take images for card art

    private TextMeshProUGUI cardName;
    private TextMeshProUGUI cost;
    private TextMeshProUGUI description;

    public void InitializeView(int id, GameObject prefab) {
        this.Id = id;
        visual = this.gameObject;

        // Deactivate the visual while linking the UI components
        visual.SetActive(false);
        cardName = visual.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        cost = visual.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        description = visual.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();

        // Move the card to hand
        VisualController.SharedInstance.ParentToHand(visual.transform);

        // Ensure the real card doesn't have lingering effects from an old card
        visual.transform.localScale = new Vector3(1, 1, 1);
        // Set the position roughly to where the "deck" is so it animates to hand from that location
        visual.transform.localPosition = new Vector3(-VisualController.SharedInstance.GetDisplaySize().x / 3f, 0, 0);
        visual.transform.rotation = Quaternion.identity;

        // Set the CardInteraction parameters for the visual to hook up interaction
        CardControl control = visual.GetComponent<CardControl>();
        control.cardId = id;
        control.hand = VisualController.SharedInstance.GetHand().GetComponent<CurvedLayout>();
        visual.SetActive(true);
    }

    // Function to call for a card that has choices needing resolution before the card can be played
    public void StageCard() {
        visual.SetActive(false);
        // Move the card to the "Staging Area position" if it isn't already there
        VisualController.SharedInstance.ParentToDisplayCanvas(visual.transform);
        visual.SetActive(true);
        LeanTween.moveLocal(visual, new Vector3(0, -200, 0), 0.2f);
    }

    public void SetName(string name) {
        this.cardName.text = name;
    }

    public void SetDescription(string description) {
        this.description.text = description;
    }

    public void SetCost(int cost) {
        this.cost.text = cost.ToString();
        // this.dummyCost.text = cost.ToString();
    }

    // It's a good idea to deactivate visuals before making updates to an object because
    // the process of "dirtying" the object forces Unity to make expensive render calls on changes
    // that may not even show up after all changes have occurred.
    public void SetActive(bool active = true) {
        visual.SetActive(active);
    }

    public void Despawn() {
        ObjectPooler.Despawn(visual);
    }
}
