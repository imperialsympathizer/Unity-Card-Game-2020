using System.Collections.Generic;
using System.Linq;

public static class SummonController {
    // Dictionary is used for searching for a specific summon by Id (when targeting, etc.)
    // List is used for resolving things like combat, where iteration is important
    private static Dictionary<int, Summon> summonDictionary = new Dictionary<int, Summon>();

    // Cache the index of a summon whenever it is attacked
    private static int index;

    public static void Initialize() {
    }

    public static void CreateSummon(Summon.Summonable summonType) {
        if (PlayerController.GetSlotsValue() - summonDictionary.Count > 0) {
            Summon newSummon;
            switch (summonType) {
                case Summon.Summonable.ZOMBIE:
                    newSummon = new Summon("Zombie", VisualController.SharedInstance.GetPrefab("ZombiePrefab"), 1, 1, 1, 1);
                    StaticEffectController.AddStatus(new Infector(newSummon, 1));
                    // TODO: add attack effect to the zombie so that any enemy hit by it takes infection
                    // TODO: add death effect to the zombie so that any enemy that kills it takes infection
                    break;
                case Summon.Summonable.SKELETON:
                    newSummon = new Summon("Skeleton", VisualController.SharedInstance.GetPrefab("SkeletonPrefab"), 3, 2, 10, 10);
                    break;
                case Summon.Summonable.SPIRIT:
                    newSummon = new Summon("Spirit", VisualController.SharedInstance.GetPrefab("SpiritPrefab"), 0, 0);
                    StaticEffectController.AddStatus(new CostReducer(newSummon, -5));
                    break;
                default:
                    newSummon = new Summon("Zombie", VisualController.SharedInstance.GetPrefab("ZombiePrefab"), 1, 1, 1, 1);
                    // Default summon is zombie
                    break;
            }
            newSummon.CreateVisual();
            summonDictionary.Add(newSummon.id, newSummon);
        }
    }

    public static void UpdateLife(int summonId, int val) {
        Summon summon = summonDictionary[summonId];
        if (summon != null) {
            summon.UpdateLifeValue(val);
            summon.UpdateVisual();
            if (summon.CheckDeath()) {
                summon.ClearVisual();
                summonDictionary.Remove(summonId);
            }
            else {
                summonDictionary[summonId] = summon;
            }
        }
    }

    public static List<Summon> GetSummonList() {
        List<Summon> summons = new List<Summon>();
        foreach (KeyValuePair<int, Summon> summonEntry in summonDictionary) {
            if (summonEntry.Value != null) {
                summons.Add(summonEntry.Value);
            }
        }
        return summons;
    }

    public static Summon GetDefender() {
        // This function returns null if there are no summons available to take damage from an attack
        // Otherwise, the front summon is returned
        index = 0;
        if (summonDictionary.Count < 1) {
            return null;
        }

        // Only attack the summon if it has life
        foreach (KeyValuePair<int, Summon> summonEntry in summonDictionary.Reverse()) {
            if (summonEntry.Value.HasLife) {
                return summonEntry.Value;
            }
        }

        return null;
    }

    public static void CompleteAttack(int summonId, Fighter attacker) {
        // Change the life total to reflect damage taken
        Summon defender = summonDictionary[summonId];
        if (defender != null) {
            defender.ReceiveAttack(attacker);

            // If damage exceeds the life remaining, the summon is defeated
            // Otherwise, update the life total and visuals
            if (defender.LifeValue <= 0) {
                // TODO: death animation
                // Clear the visual first to ensure proper removal
                defender.ClearVisual();
                summonDictionary.Remove(defender.id);
            }
            else {
                defender.UpdateVisual();
                // Update the summon objects in the list and dictionary
                summonDictionary[defender.id] = defender;
            }
        }
    }

    private static void UpdateVisual(int id) {
        // Updates any visuals that display player data
        Summon editSummon;
        if (summonDictionary.TryGetValue(id, out editSummon)) {
            editSummon.UpdateVisual();
        }
    }
}
