using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetSelector : MonoBehaviour, IPointerClickHandler {
    public static TargetSelector Instance;

    private GameObject targetCanvas;
    private GameObject shadow;
    private TextMeshProUGUI targetDialogue;

    // List with pairs of target ids and targetTypes
    private List<Tuple<int, Target>> selectedTargets = new List<Tuple<int, Target>>();
    private List<Tuple<BaseInteractable, Target>> selectableTargets = new List<Tuple<BaseInteractable, Target>>();

    public bool Selecting { get; private set; }
    private int minTargets;
    private int maxTargets;

    Color noColor = new Color(0, 0, 0, 0);
    Color unselectedColor = new Color(0.25f, 0.7f, 0.7f, 1);
    Color selectedColor = new Color(0, 1.6f, 0, 1);

    public static event Action<List<Tuple<int, Target>>> OnTargetingComplete;

    private void Awake() {
        Instance = this;
        Selecting = false;
        targetCanvas = GameObject.Find("TargetingCanvas");
        targetCanvas.SetActive(false);
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

        TargetsSelectedButton.OnTargetsSelectedClicked += OnTargetingDone;

        // Enable targetable characters to be selected
        for (int i = 0; i < effect.validTargets.Count; i++) {
            Target targetType = effect.validTargets[i];

            if (targetType == Target.ENEMY) {
                List<Enemy> enemies = EnemyController.Instance.GetEnemyList();
                for (int j = 0; j < enemies.Count; j++) {
                    Enemy enemy = enemies[j];
                    enemy.SetVisualOutline(unselectedColor);
                    selectableTargets.Add(new Tuple<BaseInteractable, Target>(enemy, Target.ENEMY));
                }
            }
            if (targetType == Target.SUMMON) {
                List<Summon> summons = SummonController.Instance.GetSummonList();
                for (int j = 0; j < summons.Count; j++) {
                    Summon summon = summons[j];
                    summon.SetVisualOutline(unselectedColor);
                    selectableTargets.Add(new Tuple<BaseInteractable, Target>(summon, Target.SUMMON));
                }
            }
            if (targetType == Target.PLAYER) {
                Player player = PlayerController.Instance.GetPlayer();
                player.SetVisualOutline(unselectedColor);
                selectableTargets.Add(new Tuple<BaseInteractable, Target>(player, Target.PLAYER));
            }
            if (targetType == Target.CARD) {
                List<Card> cards = CardManager.Instance.GetHandCards();
                for (int j = 0; j < cards.Count; j++) {
                    Card card = cards[j];
                    card.SetVisualOutline(unselectedColor);
                    selectableTargets.Add(new Tuple<BaseInteractable, Target>(card, Target.SUMMON));
                }
            }
        }

        // Start the target selection
        Selecting = true;
    }

    public void DisableTargeting() {
        foreach (Tuple<BaseInteractable, Target> target in selectableTargets) {
            target.Item1.SetVisualOutline(noColor);
        }

        targetCanvas.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (Selecting) {
            Vector3 mousePos = VisualController.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
            for (int i = 0; i < selectableTargets.Count; i++) {
                BaseInteractable selectable = selectableTargets[i].Item1;
                RectTransform selectableArea = selectable.GetVisualRect();
                float halfWidth = selectableArea.sizeDelta.x / 2;
                float halfHeight = selectableArea.sizeDelta.y / 2;
                Vector3 relativePos = selectableArea.transform.InverseTransformPoint(mousePos);
                // If the click was inside the area of the selectable object, select it
                if (relativePos.x <= halfWidth && relativePos.y <= halfHeight && relativePos.x >= -halfWidth && relativePos.x >= -halfHeight) {
                    Tuple<int, Target> selectedItem = new Tuple<int, Target>(selectable.Id, selectableTargets[i].Item2);
                    // If the object was already selected, deselect it
                    if (selectedTargets.Contains(selectedItem)) {
                        selectable.SetVisualOutline(unselectedColor);
                        selectedTargets.Remove(selectedItem);
                    }
                    else if (selectedTargets.Count < maxTargets) {
                        // Otherwise, if selectedTargets count is less than maxTargets, select it
                        selectable.SetVisualOutline(selectedColor);
                        selectedTargets.Add(selectedItem);
                    }
                }
            }
        }
    }

    private void OnTargetingDone() {
        if (Selecting) {
            if ((minTargets > 0 && selectedTargets.Count >= minTargets) || minTargets == 0) {
                Selecting = false;
                DisableTargeting();
                TargetsSelectedButton.OnTargetsSelectedClicked -= OnTargetingDone;
                OnTargetingComplete?.Invoke(selectedTargets);
            }
        }
    }
}
