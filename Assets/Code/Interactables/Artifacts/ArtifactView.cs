using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactView : BaseView {
    private SpriteRenderer icon;

    // height of this object is set to 42x the number of element types
    private RectTransform tooltipDimensions;

    private Dictionary<Element.ElementType, GameObject> elementThresholds;

    public ArtifactView(GameObject visual, Artifact artifactSource) : base(visual, artifactSource.Id) {
        this.visual.SetActive(false);
        VisualController.Instance.ParentToArtifactCanvas(this.visual.transform);
        this.visual.transform.localScale = Vector3.one;
        this.visual.transform.localPosition = Vector3.zero;

        this.icon = this.visual.transform.GetChild(0).GetComponent<SpriteRenderer>();
        this.icon.sprite = VisualController.Instance.GetImage(artifactSource.name);
        this.tooltipDimensions = this.visual.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<RectTransform>();

        elementThresholds = new Dictionary<Element.ElementType, GameObject>();

        // Create a display for each required element type
        foreach (Element.ElementType requiredType in artifactSource.typesRequired) {
            GameObject newThreshold = ObjectPooler.Spawn(VisualController.Instance.GetPrefab("Element Threshold"), Vector3.zero, Quaternion.identity);
            // Set icon
            newThreshold.transform.GetChild(0).GetComponent<Image>().sprite = VisualController.Instance.GetImage(Element.GetElementString(requiredType));
            // Set parent and scale
            newThreshold.transform.SetParent(visual.transform.GetChild(1).GetChild(1), true);
            newThreshold.transform.localScale = Vector3.one;
            newThreshold.transform.localPosition = Vector3.zero;

            elementThresholds.Add(requiredType, newThreshold);
        }

        UpdateElementCounts(artifactSource.elementsRequired);

        // Set tooltip box size based on number of element types
        tooltipDimensions.sizeDelta = new Vector2(tooltipDimensions.sizeDelta.x, 42 * artifactSource.elementsRequired.Count);
        this.visual.SetActive(true);
        DisplayTooltip();
    }

    public void DisplayTooltip() {
        visual.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void HideTooltip() {
        visual.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void UpdateElementCounts(Dictionary<Element.ElementType, Tuple<int, int>> elementsRequired) {
        foreach (KeyValuePair<Element.ElementType, Tuple<int, int>> requiredElement in elementsRequired) {
            if (elementThresholds.TryGetValue(requiredElement.Key, out GameObject thresholdDisplay)) {
                // Current
                thresholdDisplay.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = requiredElement.Value.Item2.ToString();
                // Required
                thresholdDisplay.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = requiredElement.Value.Item1.ToString();
            }
        }
    }
}
