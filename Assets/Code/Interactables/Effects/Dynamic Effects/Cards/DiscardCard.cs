public class DiscardCard : DynamicEffect {
    public DiscardCard(int effectCount) : base(effectCount) {
    }

    public override bool IsValid() {
        return (id >= 0 && effectCount >= 0);
    }

    public override void ResolveEffect() {
        for (int i = 0; i < effectCount; i++) {
            CardManager.SharedInstance.DiscardRandomCard();
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
