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
        if (PlayerController.SharedInstance.GetAvailableSlots() - summonList.Count > 0) {
            Summon newSummon = new Summon(summonType, ResourceController.GenerateId());
            newSummon.CreateVisual();
            summonDictionary.Add(newSummon.id, newSummon);
            summonList.Add(newSummon);
        }
    }

    public bool ResolveAttack(int damage) {
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
        int lifeResult = summon.LifeValue - damage;

        // If damage exceeds the life remaining, the summon is defeated
        // Otherwise, update the life total and visuals
        if (lifeResult <= 0) {
            // TODO: death animation
            // Clear the visual first to ensure proper removal
            summon.ClearVisual();
            summonList.RemoveAt(summonIndex);
            summonDictionary.Remove(summon.id);
        }
        else {
            summon.LifeValue = lifeResult;
            // Update the summon objects in the list and dictionary
            summonList[summonIndex] = summon;
            summonDictionary[summon.id] = summon;
            // Update the list
            summon.UpdateVisual();
        }

        return true;
    }

    public Queue<int> GetAttackQueue() {
        // Returns a queue (FIFO) of attacks performed by summons
        // Each summon will add its attackValue to a new entry in the queue as many times as their attackTimes is
        Queue<int> attacks = new Queue<int>();
        for (int i = 0; i < summonList.Count; i++) {
            Summon summon = summonList[i];
            if (summon.AttackTimes > 0) {
                for (int j = 0; j < summon.AttackTimes; j++) {
                    attacks.Enqueue(summon.AttackValue);
                }
            }
        }

        return attacks;
    }

    private void UpdateVisual(int id) {
        // Updates any visuals that display player data
        Summon editSummon;
        if (summonDictionary.TryGetValue(id, out editSummon)) {
            editSummon.UpdateVisual();
        }
    }
}
