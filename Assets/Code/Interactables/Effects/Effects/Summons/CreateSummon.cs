using UnityEngine;

public class CreateSummon : PlayEffect {
    // The type of summon to create
    private readonly Summon.Summonable summonType;

    public CreateSummon(int repeatCount, Summon.Summonable summonType) : base(repeatCount) {
        this.summonType = summonType;
    }

    public override void ResolveEffect() {
        Debug.Log("Summoning " + repeatCount.ToString() + " summons.");
        for (int i = 0; i < repeatCount; i++) {
            SummonController.SharedInstance.CreateSummon(summonType);
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        EffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
