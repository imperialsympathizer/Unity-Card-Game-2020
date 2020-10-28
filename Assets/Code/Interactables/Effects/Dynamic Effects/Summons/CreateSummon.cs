using System;

[Serializable]
public class CreateSummon : DynamicEffect {
    // The type of summon to create
    private readonly Summon.Summonable summonType;

    public CreateSummon(int effectCount, Summon.Summonable summonType) : base(effectCount) {
        this.summonType = summonType;
    }

    public override bool IsValid() {
        return (Id >= 0 && effectCount >= 0);
    }

    public override void ResolveEffect() {
        // Debug.Log("Summoning " + effectCount.ToString() + " summons.");
        for (int i = 0; i < effectCount; i++) {
            SummonController.Instance.CreateSummon(summonType);
        }

        // After resolving effects, remove event listener then fire OnEffectComplete
        DynamicEffectController.OnEffectBegin -= ResolveEffect;
        EffectCompleteEvent();
    }
}
