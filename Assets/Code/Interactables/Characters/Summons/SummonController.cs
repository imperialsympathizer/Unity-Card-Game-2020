using System.Collections.Generic;
using UnityEngine;

public class SummonController {
    public static SummonController SharedInstance;

    // Dictionary is used for searching for a specific summon by Id (when targeting, etc.)
    // List is used for resolving things like combat, where iteration is important
    private Dictionary<int, Summon> summonDictionary = new Dictionary<int, Summon>();
    private List<Summon> summonList = new List<Summon>();

    public void Initialize() {
        SharedInstance = this;
    }

    public void CreateSummon(Summon.Summonable summonType) {
        if (PlayerController.SharedInstance.GetSlotsValue() - summonList.Count > 0) {
            Summon newSummon;
            switch (summonType) {
                case Summon.Summonable.ZOMBIE:
                    newSummon = new Summon("Zombie", VisualController.SharedInstance.GetPrefab("ZombiePrefab"), 1, 1, 1, 1);
                    StaticEffectController.SharedInstance.AddStatus(new Infector(newSummon, 1));
                    // TODO: add attack effect to the zombie so that any enemy hit by it takes infection
                    // TODO: add death effect to the zombie so that any enemy that kills it takes infection
                    break;
                case Summon.Summonable.SKELETON:
                    newSummon = new Summon("Skeleton", VisualController.SharedInstance.GetPrefab("SkeletonPrefab"), 3, 2, 10, 10);
                    break;
                case Summon.Summonable.SPIRIT:
                    newSummon = new Summon("Spirit", VisualController.SharedInstance.GetPrefab("SpiritPrefab"), 0, 0);
                    // TODO: add effect to the spirit that reduces all enemy attack values
                    break;
                default:
                    newSummon = new Summon("Zombie", VisualController.SharedInstance.GetPrefab("ZombiePrefab"), 1, 1, 1, 1);
                    // Default summon is zombie
                    break;
            }
            newSummon.CreateVisual();
            summonDictionary.Add(newSummon.id, newSummon);
            summonList.Add(newSummon);
        }
    }

    public void PerformAttacks() {
        // Iterate from right to left
        for (int i = summonList.Count - 1; i >= 0; i--) {
            Summon attacker = summonList[i];
            if (attacker.AttackTimes > 0 && attacker.AttackValue > 0) {
                attacker.PerformAttacks();
            }
        }
    }

    public bool ReceiveAttack(Attacker attacker) {
        // This function returns false if there are no summons available to take damage from an attack
        // Otherwise, damage is dealt to the front summon (at the end of the list)
        int summonIndex = summonList.Count - 1;
        if (summonIndex < 0) {
            return false;
        }

        // Only attack the summon if it has life
        // Otherwise, attack the next summon
        Summon summon = summonList[summonIndex];
        while (!summon.HasLife) {
            summonIndex--;
            if (summonIndex < 0) {
                return false;
            }

            summon = summonList[summonIndex];
        }

        // Change the life total to reflect damage taken
        summon.ReceiveAttack(attacker);

        // If damage exceeds the life remaining, the summon is defeated
        // Otherwise, update the life total and visuals
        if (summon.LifeValue <= 0) {
            // TODO: death animation
            // Clear the visual first to ensure proper removal
            summon.ClearVisual();
            summonList.RemoveAt(summonIndex);
            summonDictionary.Remove(summon.id);
        }
        else {
            summon.UpdateVisual();
            // Update the summon objects in the list and dictionary
            summonList[summonIndex] = summon;
            summonDictionary[summon.id] = summon;
        }

        return true;
    }

    private void UpdateVisual(int id) {
        // Updates any visuals that display player data
        Summon editSummon;
        if (summonDictionary.TryGetValue(id, out editSummon)) {
            editSummon.UpdateVisual();
        }
    }
}
