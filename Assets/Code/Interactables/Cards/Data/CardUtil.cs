using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CardUtil {
    public static List<Card> LoadCardsFromJson() {
        List<Card> cards = new List<Card>();
        try {
            // get the json string of the file
            string jsonString = Resources.Load<TextAsset>("Cards/Cards").text;

            // Deserialize the json to a CardSource
            CardSourceDTO source = JsonConvert.DeserializeObject<CardSourceDTO>(jsonString);

            foreach (CardDTO cardDTO in source.Cards) {
                // Create a new card object for each DTO in the source
                // First need to reflectively create each effect object for the card
                List<DynamicEffect> cardEffects = CreateCardEffects(cardDTO);

                cards.Add(new Card(cardDTO.Name, cardDTO.Description, cardDTO.LifeCost, cardDTO.Rarity, cardEffects));
            }
        }
        catch (Exception e) {
            throw new Exception($"Error while loading cards from json: {e}");
        }

        return cards;
    }

    private static List<DynamicEffect> CreateCardEffects(CardDTO cardDTO) {
        List<DynamicEffect> cardEffects = new List<DynamicEffect>();

        foreach (EffectDTO effectDTO in cardDTO.Effects) {
            // Get the class type of the effect to create
            Type effectType = Type.GetType(effectDTO.EffectType);

            // Get the first parametrized public constructor (this assumes effects only have 1!)
            var parametrizedCtor = effectType
                    .GetConstructors()
                    .FirstOrDefault(c => c.GetParameters().Length > 0);

            if (parametrizedCtor != null) {
                object newEffect = parametrizedCtor.Invoke
                    (parametrizedCtor.GetParameters()
                    .Select(p =>
                    effectDTO.GetType()
                    .GetField(Char.ToUpperInvariant(p.Name[0]) + p.Name.Substring(1))
                    .GetValue(effectDTO)
                    ).ToArray()
                    );

                if (newEffect != null) {
                    if (newEffect is DynamicEffect dynamic) {
                        cardEffects.Add(dynamic);
                    }
                    else {
                        throw new Exception($"The created object is not a DynamicEffect");
                    }
                }
                else {
                    throw new Exception($"Unable to instantiate effect object for effect of type {effectType.Name}");
                }
            }
            else {
                throw new Exception($"Unable to retrieve constructor for effect of type {effectType.Name}");
            }
        }

        if (cardEffects.Count == 0) {
            throw new Exception($"No effects were created for card: {cardDTO.Name}");
        }

        return cardEffects;
    }
}