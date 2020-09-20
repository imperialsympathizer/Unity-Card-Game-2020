using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour {
    private GameObject visual;

    private int Id;

    // TODO: add an object to the card that can take images for card art

    private TextMeshProUGUI cardName;
    private TextMeshProUGUI cost;
    private TextMeshProUGUI description;

    // This is our placeholder object for the purposes of keeping hand size constant while moving a card around
    // Can also provide a shadow effect when used in hand by setting the alpha to ~0.5f
    private GameObject dummy;

    private TextMeshProUGUI dummyName;
    private TextMeshProUGUI dummyCost;
    private TextMeshProUGUI dummyDescription;

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

        // Since regular and dummy card objects both come from the same pool, ensure the real card doesn't have lingering effects from an old card
        visual.transform.localScale = new Vector3(1, 1, 1);
        visual.GetComponent<CanvasGroup>().alpha = 1;
        visual.GetComponent<CardMouseInteraction>().enabled = true;

        // Initialize the dummy card, but do not make it visible
        // Dummy card is only necessary for dragging and zooming
        dummy = ObjectPooler.Spawn(prefab, new Vector3(0, 0, 0), Quaternion.identity);

        dummy.SetActive(false);
        dummyName = dummy.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        dummyCost = dummy.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        dummyDescription = dummy.transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();

        // Remove the card control component from the dummy
        dummy.GetComponent<CardMouseInteraction>().enabled = false;

        // Set the CardInteraction parameters for the visual to hook up interaction
        CardMouseInteraction control = visual.GetComponent<CardMouseInteraction>();
        control.cardId = id;
        control.placeholder = dummy;
        control.handTransform = visual.transform.parent;
        visual.SetActive(true);
    }

    public void SetName(string name) {
        this.cardName.text = name;
        this.dummyName.text = name;
    }

    public void SetDescription(string description) {
        this.description.text = description;
        this.dummyDescription.text = description;
    }

    public void SetCost(int cost) {
        this.cost.text = cost.ToString();
        this.dummyCost.text = cost.ToString();
    }

    // It's a good idea to deactivate visuals before making updates to an object because
    // the process of "dirtying" the object forces Unity to make expensive render calls on changes
    // that may not even show up after all changes have occurred.
    public void SetActive(bool active = true) {
        visual.SetActive(active);
    }

    public void Despawn() {
        ObjectPooler.Despawn(visual);
        ObjectPooler.Despawn(dummy);
    }
}
