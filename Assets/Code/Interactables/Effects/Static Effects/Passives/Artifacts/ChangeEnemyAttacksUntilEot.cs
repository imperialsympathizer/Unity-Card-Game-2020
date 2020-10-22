using System;
using System.Collections.Generic;

public class ChangeEnemyAttacksUntilEot : Passive {
    // Keyed by element, first tuple value is amount required, second tuple value is current stored amount
    private Dictionary<Element.ElementType, Tuple<int, int>> elementsRequired;

    public ChangeEnemyAttacksUntilEot(int effectCount, List<Element> elementsRequired)
        : base(
            effectCount,
            new List<Trigger> { // List of triggers for the effect
                new OnTurnElementUpdate(new List<Trigger.TriggerAction> { // Subscribe to the OnTurnElementUpdate trigger
                    Trigger.TriggerAction.RESOLVE // Execute ResolveEffect() when triggered
                })
            },
            0) {
        // Initialize required elements
        this.elementsRequired = new Dictionary<Element.ElementType, Tuple<int, int>>();
        foreach (Element element in elementsRequired) {
            if (this.elementsRequired.TryGetValue(element.type, out Tuple<int, int> elementValues)) {
                this.elementsRequired[element.type] = new Tuple<int, int>(elementValues.Item1 + element.count, 0);
            }
            else {
                this.elementsRequired.Add(element.type, new Tuple<int, int>(element.count, 0));
            }
        }
    }

    protected override void OperateOnEffect(Trigger trigger) { }

    protected override void ResolveEffect(Trigger trigger) {
        if (trigger != null && trigger is OnTurnElementUpdate elementUpdate && elementUpdate.elements != null) {
            UpdateElementCount(elementUpdate.elements);
            // Trigger for as many times as the threshold is met
            while (CheckThresholdMet()) {
                PerformEffect();

                ReduceCountsByThreshold();
            }
        }
    }

    private void PerformEffect() {
        foreach (Enemy enemy in EnemyController.Instance.GetEnemyList()) {
            // The Dynamic effect immediately changes the value and the static effect resets the value at EOT
            DynamicEffectController.Instance.AddEffect(new ChangeFighterAttackValue(effectCount, enemy));
            StaticEffectController.Instance.AddModifier(enemy, new ChangeFighterAttackEot(-effectCount));
        }
    }

    private void UpdateElementCount(List<Element> elements) {
        // add each element (if relevant) to the current element tally
        foreach (Element element in elements) {
            if (elementsRequired.TryGetValue(element.type, out Tuple<int, int> elementValues)) {
                elementsRequired[element.type] = new Tuple<int, int>(elementValues.Item1, elementValues.Item2 + element.count);
            }
        }
    }

    private bool CheckThresholdMet() {
        bool thresholdMet = true;
        foreach (KeyValuePair<Element.ElementType, Tuple<int, int>> elementRequired in elementsRequired) {
            if (elementRequired.Value.Item1 > elementRequired.Value.Item2) {
                thresholdMet = false;
                break;
            }
        }

        return thresholdMet;
    }

    private void ReduceCountsByThreshold() {
        foreach (KeyValuePair<Element.ElementType, Tuple<int, int>> elementRequired in elementsRequired) {
            elementsRequired[elementRequired.Key] = new Tuple<int, int>(elementRequired.Value.Item1, elementRequired.Value.Item2 - elementRequired.Value.Item1);
        }
    }
}