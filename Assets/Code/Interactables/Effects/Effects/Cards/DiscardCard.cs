using UnityEngine;

public class DiscardCard : PlayEffect {
    public DiscardCard(int effectCount) : base(effectCount) {
    }

    public override void ResolveEffect() {
        Debug.Log("amount:" + effectCount.ToString());
        for (int i = 0; i < effectCount; i++) {
            CardManager.SharedInstance.DiscardRandomCard();
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        EffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
