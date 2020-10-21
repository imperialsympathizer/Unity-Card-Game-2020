using System.Collections.Generic;

public class ChangeCardCost : TargetableDynamicEffect {
    // This dynamic effect changes the cost of a card in hand by effectCount

    public ChangeCardCost(int effectCount,
        string targetingDialogue,
        int minTargets,
        int maxTargets)
        : base(effectCount, new List<Target> { Target.CARD }, targetingDialogue, minTargets, maxTargets) { }

    public override bool IsValid() {
        return (id >= 0 && effectCount != 0 && selectedTargets != null && selectedTargets.Count > 0);
    }

    public override void ResolveEffect() {
        for (int i = 0; i < selectedTargets.Count; i++) {
            Card updatedCard = CardManager.Instance.GetHandCardById(selectedTargets[i].Item1);
            updatedCard.UpdateLifeCost(effectCount);
            CardManager.Instance.UpdateHandCard(updatedCard);
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}