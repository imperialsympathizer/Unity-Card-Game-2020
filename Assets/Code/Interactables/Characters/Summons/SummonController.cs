using System.Collections.Generic;
using System.Linq;

public class SummonController : BaseController {
    public static SummonController Instance;

    // Dictionary is used for searching for a specific summon by Id (when targeting, etc.)
    // List is used for resolving things like combat, where iteration is important
    private Dictionary<int, Summon> summonDictionary = new Dictionary<int, Summon>();

    protected override bool Initialize() {
        Instance = this;
        return true;
    }

    public void CreateSummon(Summon.Summonable summonType) {
        if (PlayerController.Instance.GetSlotsValue() - summonDictionary.Count > 0) {
            Summon newSummon;
            switch (summonType) {
                case Summon.Summonable.ZOMBIE:
                default:
                    newSummon = new Summon("Zombie", VisualController.Instance.GetPrefab("ZombiePrefab"), 1, 1, 1, 1);
                    StaticEffectController.Instance.AddStatus(newSummon, new Infector(1));
                    break;
                case Summon.Summonable.SKELETON:
                    newSummon = new Summon("Skeleton", VisualController.Instance.GetPrefab("SkeletonPrefab"), 3, 2, 10, 10);
                    break;
                case Summon.Summonable.SPIRIT:
                    newSummon = new Summon("Spirit", VisualController.Instance.GetPrefab("SpiritPrefab"), 0, 0);
                    StaticEffectController.Instance.AddStatus(newSummon, new CardCostChanger(-5));
                    break;
                case Summon.Summonable.MUMMY:
                    newSummon = new Summon("Mummy", VisualController.Instance.GetPrefab("MummyPrefab"), 0, 0, 5, 5);
                    StaticEffectController.Instance.AddStatus(newSummon, new Cursed(10));
                    break;
            }
            newSummon.CreateVisual();
            summonDictionary.Add(newSummon.id, newSummon);
        }
    }

    #region Update Methods
    public void UpdateMaxLife(int summonId, int val) {
        Summon summon = GetSummon(summonId);
        if (summon != null) {
            if (summon.UpdateMaxLife(val)) {
                summon.ClearVisual();
                summonDictionary.Remove(summonId);
            }
        }
    }

    public void UpdateLife(int summonId, int val) {
        Summon summon = GetSummon(summonId);
        if (summon != null) {
            // If the summon dies from this change, remove it
            if (summon.UpdateLifeValue(val)) {
                summon.ClearVisual();
                summonDictionary.Remove(summonId);
            }
        }
    }

    public void UpdateAttackValue(int summonId, int val) {
        Summon summon = GetSummon(summonId);
        if (summon != null) {
            summon.UpdateAttackValue(val);
        }
    }

    public void UpdateAttackTimes(int summonId, int val) {
        Summon summon = GetSummon(summonId);
        if (summon != null) {
            summon.UpdateAttackTimes(val);
        }
    }
    #endregion

    #region Getters
    public List<Summon> GetSummonList() {
        List<Summon> summons = new List<Summon>();
        foreach (KeyValuePair<int, Summon> summonEntry in summonDictionary) {
            if (summonEntry.Value != null) {
                summons.Add(summonEntry.Value);
            }
        }
        return summons;
    }

    public Summon GetSummon(int summonId) {
        // This function returns null if the requested enemy does not exist
        if (summonDictionary.TryGetValue(summonId, out Summon summon)) {
            return summon;
        }

        return null;
    }

    public Summon GetDefender() {
        // This function returns null if there are no summons available to take damage from an attack
        // Otherwise, the front summon is returned
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

    public Summon GetRandomSummon(bool hasLife = false) {
        // This function returns null if there are no summons available
        if (summonDictionary.Count < 1) {
            return null;
        }

        List<Summon> summonsWithLife = GetSummonList();

        if (hasLife) {
            foreach (Summon summon in summonsWithLife) {
                if (!summon.HasLife) {
                    summonsWithLife.Remove(summon);
                }
            }
        }

        return summonsWithLife[RandomNumberGenerator.getRandomIndexFromRange(summonsWithLife.Count - 1)];
    }
    #endregion

    public void CompleteAttack(int summonId, Fighter attacker) {
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
}
