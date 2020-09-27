using UnityEngine;

public class DrawCard : PlayEffect {
    public DrawCard(int repeatCount) : base(repeatCount) {
    }

    public override void ResolveEffect() {
        Debug.Log("Drawing " + repeatCount.ToString() + " cards.");
        for (int i = 0; i < repeatCount; i++) {
            CardManager.SharedInstance.DrawCard();
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        EffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
