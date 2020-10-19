using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : BaseView {
    // image of the whole card (not just art)
    private Image cardImage;

    // TODO: add an object to the card that can take images for card art

    private TextMeshProUGUI cardName;
    private TextMeshProUGUI cost;
    private TextMeshProUGUI description;

    public CardView(GameObject visual, int id) : base(visual, id) {
        // Deactivate the visual while linking the UI components
        visual.SetActive(false);
        cardImage = visual.transform.GetChild(1).GetComponent<Image>();
        cardName = cardImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        cost = cardImage.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        description = cardImage.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        // Move the card to hand
        MoveToHand();

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

    public void MoveToHand() {
        VisualController.SharedInstance.ParentToHand(visual.transform);
    }

    public RectTransform GetVisualRect() {
        return visual.GetComponent<RectTransform>();
    }

    public void SetVisualOutline(Color color) {
        cardImage.material.SetColor("_OutlineColor", color);
    }

    public void SetName(string name) {
        this.cardName.text = name;
    }

    public void SetDescription(string description) {
        this.description.text = description;
    }

    public void SetCost(int cost) {
        this.cost.text = cost.ToString();
        // NumberAnimator.AnimateNumberChange(this.cost, cost);
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
