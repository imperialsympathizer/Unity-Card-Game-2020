using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class CardUtil {
    // This class' main function is for serialization/deserialization of card data to/from json files

    public static List<Card> LoadCardsFromJson() {
        List<Card> cards = new List<Card>();
        try {
            // get the json string of the file
            string jsonString = Resources.Load<TextAsset>("Cards/Cards").text;

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
}