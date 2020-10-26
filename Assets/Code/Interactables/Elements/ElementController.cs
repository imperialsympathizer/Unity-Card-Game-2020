using System;
using System.Collections.Generic;
using UnityEngine;

public class ElementController : BaseController {
    public static ElementController Instance;

    // Keep a cache of element sprites
    private Dictionary<Element.ElementType, Sprite> elementImages;

    private GameObject prefab;

    // This is the element totals for cards in the player's deck/discard/exile/hand
    // This includes temporary cards that exist only for the duration of the battle
    private Dictionary<Element.ElementType, int> totalElements;

    // This is the element totals for elements that have been played this turn
    private Dictionary<Element.ElementType, int> turnElements;

    public static event Action<Dictionary<Element.ElementType, int>> OnTotalElementUpdate;
    public static event Action<List<Element>> OnTurnElementUpdate;

    protected override bool Initialize(bool reinitialize) {
        Instance = this;
        // CardManager and VisualController need to be initialized
        if (CardManager.Instance != null && CardManager.Instance.Initialized &&
            VisualController.Instance != null && VisualController.Instance.Initialized) {
            prefab = VisualController.Instance.GetPrefab("Element Icon");

            totalElements = new Dictionary<Element.ElementType, int>();
            turnElements = new Dictionary<Element.ElementType, int>();

            if (!reinitialize) {
                elementImages = new Dictionary<Element.ElementType, Sprite>();
                elementImages.Add(Element.ElementType.AIR, VisualController.Instance.GetImage(Element.GetElementString(Element.ElementType.AIR)));
                elementImages.Add(Element.ElementType.EARTH, VisualController.Instance.GetImage(Element.GetElementString(Element.ElementType.EARTH)));
                elementImages.Add(Element.ElementType.FIRE, VisualController.Instance.GetImage(Element.GetElementString(Element.ElementType.FIRE)));
                elementImages.Add(Element.ElementType.WATER, VisualController.Instance.GetImage(Element.GetElementString(Element.ElementType.WATER)));
                elementImages.Add(Element.ElementType.LIFE, VisualController.Instance.GetImage(Element.GetElementString(Element.ElementType.LIFE)));
                elementImages.Add(Element.ElementType.DEATH, VisualController.Instance.GetImage(Element.GetElementString(Element.ElementType.DEATH)));
                elementImages.Add(Element.ElementType.ARTIFICE, VisualController.Instance.GetImage(Element.GetElementString(Element.ElementType.ARTIFICE)));
            }

            ResetTotalElements();

            return true;
        }

        return false;
    }

    public GameObject SpawnElementView(Element.ElementType type, Transform parent) {
        GameObject newIcon = ObjectPooler.Spawn(prefab, Vector3.zero, Quaternion.identity);
        SetElementView(newIcon, type, parent);
        return newIcon;
    }

    public void SetElementView(GameObject icon, Element.ElementType type, Transform parent) {
        icon.SetActive(false);

        // Change icon to correct image
        if (elementImages.TryGetValue(type, out Sprite image)) {
            icon.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = image;
        }

        // Set parent
        icon.transform.SetParent(parent);

        // Ensure scale and position are correct
        icon.transform.localScale = Vector3.one;
        icon.transform.localPosition = Vector3.zero;

        icon.SetActive(true);
    }

    public void SetElementView(GameObject icon, Element.ElementType type) {
        icon.SetActive(false);

        // Change icon to correct image
        if (elementImages.TryGetValue(type, out Sprite image)) {
            icon.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = image;
        }

        // Ensure scale and position are correct
        icon.transform.localScale = Vector3.one;
        icon.transform.localPosition = Vector3.zero;

        icon.SetActive(true);
    }

    public int GetTotalElementCount(Element.ElementType type) {
        if (totalElements.TryGetValue(type, out int count)) {
            return count;
        }

        return 0;
    }

    public void AddTotalElements(List<Element> elementList) {
        foreach (Element element in elementList) {
            if (totalElements.TryGetValue(element.type, out int currentValue)) {
                totalElements[element.type] = currentValue + element.count;
            }
            else {
                totalElements.Add(element.type, element.count);
            }
        }

        OnTotalElementUpdate?.Invoke(totalElements);
    }

    public void ResetTotalElements() {
        totalElements.Clear();
        foreach (Card card in CardManager.Instance.GetRunDeckList()) {
            foreach (Element element in card.GetOrderedElements()) {
                if (totalElements.TryGetValue(element.type, out int currentValue)) {
                    totalElements[element.type] = currentValue + element.count;
                }
                else {
                    totalElements.Add(element.type, element.count);
                }
            }
        }

        OnTotalElementUpdate?.Invoke(totalElements);
    }

    public int GetTurnElementCount(Element.ElementType type) {
        if (turnElements.TryGetValue(type, out int count)) {
            return count;
        }

        return 0;
    }

    public void AddTurnElements(List<Element> elementList) {
        if (elementList != null && elementList.Count > 0) {
            foreach (Element element in elementList) {
                if (turnElements.TryGetValue(element.type, out int currentValue)) {
                    turnElements[element.type] = currentValue + element.count;
                }
                else {
                    turnElements.Add(element.type, element.count);
                }
            }
            OnTurnElementUpdate?.Invoke(elementList);
        }
    }

    public void ResetTurnElements() {
        turnElements.Clear();
    }
}
