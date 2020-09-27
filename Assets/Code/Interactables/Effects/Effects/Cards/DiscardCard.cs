using UnityEngine;

public class DiscardCard : PlayEffect {
    public DiscardCard(int repeatCount) : base(repeatCount) {
    }

    public override void ResolveEffect() {
        Debug.Log("amount:" + repeatCount.ToString());
        for (int i = 0; i < repeatCount; i++) {
            CardManager.SharedInstance.DiscardRandomCard();
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        EffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
