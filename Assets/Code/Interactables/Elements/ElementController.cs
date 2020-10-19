using System.Collections.Generic;
using UnityEngine;

public static class ElementController {
    // Keep a cache of element sprites

    private static Dictionary<Element.ElementType, Sprite> elementImages;

    private static GameObject prefab;

    public static void Initialize() {
        prefab = VisualController.SharedInstance.GetPrefab("Element Icon");

        elementImages = new Dictionary<Element.ElementType, Sprite>();
        elementImages.Add(Element.ElementType.AIR, VisualController.SharedInstance.GetImage(Element.GetElementString(Element.ElementType.AIR)));
        elementImages.Add(Element.ElementType.EARTH, VisualController.SharedInstance.GetImage(Element.GetElementString(Element.ElementType.EARTH)));
        elementImages.Add(Element.ElementType.FIRE, VisualController.SharedInstance.GetImage(Element.GetElementString(Element.ElementType.FIRE)));
        elementImages.Add(Element.ElementType.WATER, VisualController.SharedInstance.GetImage(Element.GetElementString(Element.ElementType.WATER)));
        elementImages.Add(Element.ElementType.LIFE, VisualController.SharedInstance.GetImage(Element.GetElementString(Element.ElementType.LIFE)));
        elementImages.Add(Element.ElementType.DEATH, VisualController.SharedInstance.GetImage(Element.GetElementString(Element.ElementType.DEATH)));
        elementImages.Add(Element.ElementType.ARTIFICE, VisualController.SharedInstance.GetImage(Element.GetElementString(Element.ElementType.ARTIFICE)));
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
}
