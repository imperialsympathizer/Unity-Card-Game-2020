using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class ObjectFactory {
    // This class has 2 primary use cases
    // 1: Deserialization of effects from json (usually when loading card data)
    // 2: Creation of static effects by dynamic effects
    // This allows for "generic" dynamic effects that can create any static effect desired without requiring different constructors
    // Example: A dynamic effect creating an AttachedStaticEffect with 5 parameters still has the same constructor as if it were creating a Passive with 2 parameters

    public static DynamicEffect CreateDynamicEffectFromDTO(DynamicEffectDTO effectDTO) {
        try {
            // Get the class type of the effect to create
            Type effectType = Type.GetType(effectDTO.effectType);

            // Get the first parametrized public constructor (this assumes effects only have 1!)
            ConstructorInfo parametrizedCtor = GetConstructorInfo(effectType);

            // IMPORTANT: DTO should have fields with the same name as the constructor arguments
            object newObject = parametrizedCtor.Invoke(parametrizedCtor.GetParameters()
            .Select(p => {
                object parameter = effectDTO.GetType().GetField(p.Name).GetValue(effectDTO);
                // Convert StaticEffectDTOs to StaticEffects
                if (parameter is StaticEffectDTO staticEffectDTO) {
                    parameter = CreateStaticEffectFromDTO(staticEffectDTO);
                }

                return parameter;
            }).ToArray()
            );

            if (newObject != null) {
                if (newObject.GetType() == effectType && newObject is DynamicEffect dynamicEffect) {
                    return dynamicEffect;
                }
                else {
                    throw new Exception($"The created DynamicEffect is not correctly typed");
                }
            }
            else {
                throw new Exception($"Unable to instantiate DynamicEffect of type {effectType.Name}");
            }
        }
        catch (Exception e) {
            throw new Exception($"Failed to create DynamicEffect from DTO: {e}");
        }
    }

    public static StaticEffect CreateStaticEffectFromDTO(StaticEffectDTO effectDTO) {
        try {
            Type effectType = Type.GetType(effectDTO.effectType);

            ConstructorInfo parametrizedCtor = GetConstructorInfo(effectType);

            object newObject = parametrizedCtor.Invoke(parametrizedCtor.GetParameters()
            .Select(p => {
                object parameter = effectDTO.GetType().GetField(p.Name).GetValue(effectDTO);
                // Convert TriggerDTOs to Triggers
                if (parameter is List<TriggerDTO> triggerDTOs) {
                    List<Trigger> triggers = new List<Trigger>();
                    triggers.AddRange(triggerDTOs.Select(t => CreateTriggerFromDTO(t)).ToArray());
                    parameter = triggers;
                }

                return parameter;
            }).ToArray()
            );

            if (newObject != null) {
                if (newObject.GetType() == effectType && newObject is StaticEffect staticEffect) {
                    return staticEffect;
                }
                else {
                    throw new Exception($"The created StaticEffect is not correctly typed");
                }
            }
            else {
                throw new Exception($"Unable to instantiate StaticEffect of type {effectType.Name}");
            }
        }
        catch (Exception e) {
            throw new Exception($"Failed to create StaticEffect from DTO: {e}");
        }
    }

    public static Trigger CreateTriggerFromDTO(TriggerDTO triggerDTO) {
        try {
            Type triggerType = Type.GetType(triggerDTO.triggerType);

            ConstructorInfo parametrizedCtor = GetConstructorInfo(triggerType);

            object newObject = parametrizedCtor.Invoke(parametrizedCtor.GetParameters()
            .Select(p => triggerDTO.GetType().GetField(p.Name).GetValue(triggerDTO)
            ).ToArray()
            );

            if (newObject != null) {
                if (newObject.GetType() == triggerType && newObject is Trigger trigger) {
                    return trigger;
                }
                else {
                    throw new Exception($"The created Trigger is not correctly typed");
                }
            }
            else {
                throw new Exception($"Unable to instantiate Trigger of type {triggerType.Name}");
            }
        }
        catch (Exception e) {
            throw new Exception($"Failed to create Trigger from DTO: {e}");
        }
    }

    private static ConstructorInfo GetConstructorInfo(Type objectType) {
        // Get the first parametrized public constructor (this assumes the first constructor is correct!)
        ConstructorInfo parametrizedCtor = objectType
                .GetConstructors()
                .FirstOrDefault(c => c.GetParameters().Length > 0);

        if (parametrizedCtor != null) {
            return parametrizedCtor;
        }
        else {
            throw new Exception($"Unable to retrieve constructor for object of type {objectType}");
        }
    }
}