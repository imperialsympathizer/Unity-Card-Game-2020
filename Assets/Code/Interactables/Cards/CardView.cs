using System.Collections.Generic;
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

    private Transform elementsContainer;
    private List<GameObject> elements;

    public CardView(GameObject visual, int id) : base(visual, id) {
        // Deactivate the visual while linking the UI components
        visual.SetActive(false);
        AssignObjects();

        elements = new List<GameObject>();

        // Move the card to hand
        MoveToHand();

        // Ensure the card doesn't have lingering effects from an old card
        visual.transform.localScale = Vector3.one;
        // Set the position roughly to where the "deck" is so it animates to hand from that location
        visual.transform.localPosition = new Vector3(-VisualController.Instance.GetDisplaySize().x / 3f, 0, 0);
        visual.transform.rotation = Quaternion.identity;

        // Set the CardInteraction parameters for the visual to hook up interaction
        CardControl control = visual.GetComponent<CardControl>();
        control.cardId = id;
        control.hand = VisualController.Instance.GetHand().GetComponent<CurvedLayout>();
        visual.SetActive(true);
    }

    // This constructor is for creating decorative card visuals
    public CardView(GameObject visual, int id, Transform parent, float scale) : base(visual, id) {
        // Deactivate the visual while linking the UI components
        visual.SetActive(false);
        AssignObjects();
        
        elements = new List<GameObject>();

        // Move the card to the given transform
        visual.transform.SetParent(parent);
        visual.transform.localScale = new Vector3(scale, scale, 1);
        visual.transform.rotation = Quaternion.identity;

        // Set the CardSelect parameters
        CardSelect control = visual.GetComponent<CardSelect>();
        control.cardId = id;
        visual.SetActive(true);
    }

    private void AssignObjects() {
        cardImage = visual.transform.GetChild(1).GetComponent<Image>();
        cardName = cardImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        cost = cardImage.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        description = cardImage.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        elementsContainer = visual.transform.GetChild(2).transform;
    }

    public void MoveToHand() {
        VisualController.Instance.ParentToHand(visual.transform);
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

    public void SetElements(List<Element> elementList) {
        int childIndex = 0;
        foreach (Element element in elementList) {
            for (int i = 0; i < element.count; i++) {
                if (childIndex < elementsContainer.childCount - 1) {
                    ElementController.Instance.SetElementView(elementsContainer.GetChild(childIndex).gameObject, element.type);
                }
                else {
                    GameObject newIcon = ElementController.Instance.SpawnElementView(element.type, elementsContainer);
                    elements.Add(newIcon);
                }
                childIndex++;
            }
        }
    }

    // It's a good idea to deactivate visuals before making updates to an object because
    // the process of "dirtying" the object forces Unity to make expensive render calls on changes
    // that may not even show up after all changes have occurred.
    public void SetActive(bool active = true) {
        visual.SetActive(active);
    }

    public void Despawn() {
        // Despawn element icons first
        foreach (GameObject icon in elements) {
            ObjectPooler.Despawn(icon);
        }
        ObjectPooler.Despawn(visual);
    }
}
