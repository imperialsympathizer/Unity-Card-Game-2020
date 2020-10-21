using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class JsonUtil {
    // This class' main function is for serialization/deserialization of data to/from json files

    #region Cards
    public static List<Card> LoadCardsFromJson() {
        List<Card> cards = new List<Card>();
        try {
            // get the json string of the file
            string jsonString = Resources.Load<TextAsset>("Data/Cards").text;

            // Deserialize the json to a CardSource
            CardSourceDTO source = JsonConvert.DeserializeObject<CardSourceDTO>(jsonString);

            foreach (CardDTO cardDTO in source.cards) {
                // Create effect objects required for the card
                List<DynamicEffect> cardEffects = CreateCardEffects(cardDTO);

                cards.Add(new Card(cardDTO.name, cardDTO.description, cardDTO.lifeCost, cardDTO.rarity, cardDTO.uses, cardEffects, cardDTO.elements));
            }
        }
        catch (Exception e) {
            throw new Exception($"Error while loading cards from json: {e}");
        }

        return cards;
    }

    private static List<DynamicEffect> CreateCardEffects(CardDTO cardDTO) {
        List<DynamicEffect> cardEffects = new List<DynamicEffect>();

        foreach (DynamicEffectDTO effectDTO in cardDTO.effects) {
            DynamicEffect dynamicEffect = ObjectFactory.CreateDynamicEffectFromDTO(effectDTO);

            if (dynamicEffect != null) {
                cardEffects.Add(dynamicEffect);
            }
            else {
                throw new Exception($"Unable to instantiate effect object for effect {effectDTO.effectType}");
            }
        }

        if (cardEffects.Count == 0) {
            throw new Exception($"No effects were created for card: {cardDTO.name}");
        }

        return cardEffects;
    }
    #endregion

    #region Artifacts
    public static List<Artifact> LoadArtifactsFromJson() {
        List<Artifact> artifacts = new List<Artifact>();
        try {
            // get the json string of the file
            string jsonString = Resources.Load<TextAsset>("Data/Artifacts").text;

            // Deserialize the json to a CardSource
            ArtifactSourceDTO source = JsonConvert.DeserializeObject<ArtifactSourceDTO>(jsonString);

            foreach (ArtifactDTO artifactDTO in source.artifacts) {
                // Create effect objects required for the card
                List<Passive> artifactEffects = CreateArtifactEffects(artifactDTO);

                artifacts.Add(new Artifact(artifactDTO.name, artifactDTO.description, artifactDTO.rarity, artifactEffects));
            }
        }
        catch (Exception e) {
            throw new Exception($"Error while loading cards from json: {e}");
        }

        return artifacts;
    }

    private static List<Passive> CreateArtifactEffects(ArtifactDTO artifactDTO) {
        List<Passive> artifactEffects = new List<Passive>();

        foreach (StaticEffectDTO effectDTO in artifactDTO.effects) {
            StaticEffect staticEffect = ObjectFactory.CreateStaticEffectFromDTO(effectDTO);

            if (staticEffect != null && staticEffect is Passive passive) {
                artifactEffects.Add(passive);
            }
            else {
                throw new Exception($"Unable to instantiate effect object for effect {effectDTO.effectType}");
            }
        }

        if (artifactEffects.Count == 0) {
            throw new Exception($"No effects were created for artifact: {artifactDTO.name}");
        }

        return artifactEffects;
    }
    #endregion
}