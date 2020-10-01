using UnityEngine;

public class Summon : Fighter {
    // Class that houses the data for summons
    // Contains references to SummonView but does not directly control it in most instances
    // Accessed and instantiated through the SummonController
    Attacker summon;

    // Visual component of the player, stored within its own View class
    private SummonView display;
    private GameObject prefab;

    public enum Summonable {
        ZOMBIE,
        SKELETON,
        SPIRIT
    }


    // Constructor that creates the object, but does not instantiate visuals.
    // Those can be called as needed by the CreateVisual() function
    // This constructor creates an Attacker object
    // Constructor for a summon with health
    public Summon(string name,
        GameObject prefab,
        int baseAttack,
        int baseAttackTimes,
        int baseMaxLife,
        int baseLife) : base(name, baseAttack, baseAttackTimes, true, baseMaxLife, baseLife) {
        this.prefab = prefab;
    }

    // Summon without health
    public Summon(string name,
        GameObject prefab,
        int baseAttack,
        int baseAttackTimes) : base(name, baseAttack, baseAttackTimes) {
        this.prefab = prefab;
    }

    public void CreateVisual() {
        // Spawn an object to view the summon on screen
        // Not using the ObjectPooler as there is only one player character
        GameObject summonVisual = ObjectPooler.Spawn(prefab, new Vector3(0, 0, -10), Quaternion.identity);
        display = new SummonView();
        display.InitializeView(summonVisual, id);
        UpdateVisual();
    }

    // Function to call before moving object off the screen to another location (such as deck or discard)
    public void ClearVisual() {
        if (display != null) {
            display.Despawn();
            display = null;
        }
    }

    public override void UpdateVisual() {
        display.SetActive(false);
        display.SetAttack(AttackValue);
        display.SetAttackTimes(AttackValue, AttackTimes);
        display.SetMaxLife(MaxLife);
        display.SetLife(HasLife, LifeValue, MaxLife);
        display.SetActive(true);
    }

    public override void PerformAttack() {
        display.AnimateAttack();
    }
}
