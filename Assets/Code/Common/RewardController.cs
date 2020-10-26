using UnityEngine;

public class RewardController : BaseController {
    public static RewardController Instance;

    private float uncommonCardChance = 0;

    private float rareCardChance = 0;
    // This value keeps track of how long it's been since the last rare card drop and will increase the odds of a rare accordingly
    private float cardDesireSensor = 0;

    private float uncommonArtifactChance = 10;

    private float rareArtifactChance = 0;
    private float artifactDesireSensor = 0;

    protected override bool Initialize(bool reinitialize) {
        Instance = this;
        return true;
    }

    public Rarity GenerateCardRarity() {
        Rarity rarity = Rarity.COMMON;
        float randNum = Random.Range(0f, 1f);
        if (randNum >= 1f - rareCardChance - cardDesireSensor) {
            rarity = Rarity.RARE;
        }
        else if (randNum >= 1f - rareCardChance - cardDesireSensor - uncommonCardChance) {
            rarity = Rarity.UNCOMMON;
        }

        // Iterate rarities for future card rewards
        if (rarity == Rarity.RARE) {
            cardDesireSensor = 0f;
        }
        else {
            cardDesireSensor += 0.005f;
        }

        return rarity;
    }

    public Rarity GenerateArtifactRarity() {
        Rarity rarity = Rarity.COMMON;
        float randNum = Random.Range(0f, 1f);
        if (randNum >= 1f - rareArtifactChance - artifactDesireSensor) {
            rarity = Rarity.RARE;
        }
        else if (randNum >= 1f - rareArtifactChance - artifactDesireSensor - uncommonArtifactChance) {
            rarity = Rarity.UNCOMMON;
        }

        // Iterate rarities for future card rewards
        if (rarity == Rarity.RARE) {
            artifactDesireSensor = 0f;
        }
        else {
            artifactDesireSensor += 0.01f;
        }

        return rarity;
    }
}