using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardTargetSelector : MonoBehaviour, IPointerClickHandler {
    public static CardTargetSelector SharedInstance;

    private GameObject targetCanvas;
    private GameObject shadow;
    private TextMeshProUGUI targetDialogue;

    // List with pairs of target ids and targetTypes
    private List<int> selectedTargets = new List<int>();
    private List<Card> selectableTargets = new List<Card>();

    public bool selecting { get; private set; }
    private int minTargets;
    private int maxTargets;

    // TODO: make global
    Color noColor = new Color(0, 0, 0, 0);
    Color unselectedColor = new Color(0.25f, 0.7f, 0.7f, 1);
    Color selectedColor = new Color(0, 1.6f, 0, 1);

    public static event Action<List<int>> OnTargetingComplete;

    private void Awake() {
        SharedInstance = this;
        selecting = false;
        targetCanvas = GameObject.Find("TargetingCanvas");
        targetCanvas.SetActive(false);
        TargetsSelectedButton.OnTargetsSelectedClicked += OnTargetingDone;
        shadow = targetCanvas.transform.GetChild(0).gameObject;
        targetDialogue = targetCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void EnableTargeting(TargetableDynamicEffect effect) {
        this.minTargets = effect.minTargets;
        this.maxTargets = effect.maxTargets;
        selectedTargets.Clear();
        selectableTargets.Clear();
        targetDialogue.text = effect.targetingDialogue.ToUpper();
        targetCanvas.SetActive(true);

        // Enable cards in hand to be selected
        selectableTargets = CardManager.SharedInstance.GetHandCards();

        // Start the target selection
        selecting = true;
    }

    public void DisableTargeting() {
        foreach (Card target in selectableTargets) {
            target.SetVisualOutline(noColor);
        }

        targetCanvas.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (selecting) {
            Vector3 mousePos = VisualController.SharedInstance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
            for (int i = 0; i < selectableTargets.Count; i++) {
                Card selectable = selectableTargets[i];
                RectTransform selectableArea = selectable.GetVisualRect();
                float halfWidth = selectableArea.sizeDelta.x / 2;
                float halfHeight = selectableArea.sizeDelta.y / 2;
                Vector3 relativePos = selectableArea.transform.InverseTransformPoint(mousePos);
                // If the click was inside the area of the selectable object, select it
                if (relativePos.x <= halfWidth && relativePos.y <= halfHeight && relativePos.x >= -halfWidth && relativePos.x >= -halfHeight) {
                    // If the object was already selected, deselect it
                    if (selectedTargets.Contains(selectable.id)) {
                        selectable.SetVisualOutline(unselectedColor);
                        selectedTargets.Remove(selectable.id);
                    }
                    else if (selectedTargets.Count < maxTargets) {
                        // Otherwise, if selectedTargets count is less than maxTargets, select it
                        selectable.SetVisualOutline(selectedColor);
                        selectedTargets.Add(selectable.id);
                    }
                }
            }
        }
    }

    private void OnTargetingDone() {
        if (selecting) {
            if ((minTargets > 0 && selectedTargets.Count >= minTargets) || minTargets == 0) {
                selecting = false;
                DisableTargeting();
                OnTargetingComplete?.Invoke(selectedTargets);
            }
        }
    }
}
