using System;
using System.Collections.Generic;
using UnityEngine;

public static class ElementController {
    // Keep a cache of element sprites

    private static Dictionary<Element.ElementType, Sprite> elementImages;

    private static GameObject prefab;

    // This is the element totals for cards in the player's deck/discard/exile/hand
    // This includes temporary cards that exist only for the duration of the battle
    private static Dictionary<Element.ElementType, int> totalElements;

    // This is the element totals for elements that have been played this turn
    private static Dictionary<Element.ElementType, int> turnElements;

    public static void Initialize() {
        prefab = VisualController.SharedInstance.GetPrefab("Element Icon");

        totalElements = new Dictionary<Element.ElementType, int>();
        turnElements = new Dictionary<Element.ElementType, int>();

        elementImages = new Dictionary<Element.ElementType, Sprite>();
        elementImages.Add(Element.ElementType.AIR, VisualController.SharedInstance.GetImage(Element.GetElementString(Element.ElementType.AIR)));
        elementImages.Add(Element.ElementType.EARTH, VisualController.SharedInstance.GetImage(Element.GetElementString(Element.ElementType.EARTH)));
        elementImages.Add(Element.ElementType.FIRE, VisualController.SharedInstance.GetImage(Element.GetElementString(Element.ElementType.FIRE)));
        elementImages.Add(Element.ElementType.WATER, VisualController.SharedInstance.GetImage(Element.GetElementString(Element.ElementType.WATER)));
        elementImages.Add(Element.ElementType.LIFE, VisualController.SharedInstance.GetImage(Element.GetElementString(Element.ElementType.LIFE)));
        elementImages.Add(Element.ElementType.DEATH, VisualController.SharedInstance.GetImage(Element.GetElementString(Element.ElementType.DEATH)));
        elementImages.Add(Element.ElementType.ARTIFICE, VisualController.SharedInstance.GetImage(Element.GetElementString(Element.ElementType.ARTIFICE)));

        ResetTotalElements();
    }

    public static GameObject SpawnElementView(Element.ElementType type, Transform parent) {
        GameObject newIcon = ObjectPooler.Spawn(prefab, Vector3.zero, Quaternion.identity);
        SetElementView(newIcon, type, parent);
        return newIcon;
    }

    public static void SetElementView(GameObject icon, Element.ElementType type, Transform parent) {
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

    public static void SetElementView(GameObject icon, Element.ElementType type) {
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

    public static int GetTotalElementCount(Element.ElementType type) {
        if (totalElements.TryGetValue(type, out int count)) {
            return count;
        }

        return 0;
    }

    public static void AddTotalElements(List<Element> elementList) {
        foreach (Element element in elementList) {
            if (totalElements.TryGetValue(element.type, out int currentValue)) {
                totalElements[element.type] = currentValue + element.count;
            }
            else {
                totalElements.Add(element.type, element.count);
            }
        }
    }

    public static void ResetTotalElements() {
        totalElements.Clear();
        foreach (Card card in CardManager.SharedInstance.GetRunDeckCards()) {
            foreach (Element element in card.GetOrderedElements()) {
                if (totalElements.TryGetValue(element.type, out int currentValue)) {
                    totalElements[element.type] = currentValue + element.count;
                }
                else {
                    totalElements.Add(element.type, element.count);
                }
            }
        }
    }

    public static int GetTurnElementCount(Element.ElementType type) {
        if (turnElements.TryGetValue(type, out int count)) {
            return count;
        }

        return 0;
    }

    public static void AddTurnElements(List<Element> elementList) {
        foreach (Element element in elementList) {
            if (turnElements.TryGetValue(element.type, out int currentValue)) {
                turnElements[element.type] = currentValue + element.count;
            }
            else {
                turnElements.Add(element.type, element.count);
            }
        }
    }

    public static void ResetTurnElements() {
        turnElements.Clear();
    }
}
