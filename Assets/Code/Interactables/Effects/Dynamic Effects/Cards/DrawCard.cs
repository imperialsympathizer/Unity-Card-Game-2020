using System;

[Serializable]
public class DrawCard : DynamicEffect {
    public DrawCard(int effectCount) : base(effectCount) {
    }

    public override bool IsValid() {
        return (Id >= 0 && effectCount >= 0);
    }

    public override void ResolveEffect() {
        // Debug.Log("Drawing " + effectCount.ToString() + " cards.");
        for (int i = 0; i < effectCount; i++) {
            CardManager.Instance.DrawCard();
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
