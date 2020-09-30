using UnityEngine;

public class Enemy : Fighter {
    // Class that houses the data for nemies
    // Contains references to EnemyView but does not directly control it in most instances
    // Accessed and instantiated through the EnemyController

    // Visual component of the player, stored within its own View class
    private EnemyView display;
    private GameObject prefab;

    public enum EnemyType {
        KNIGHT
    }


    // Constructor that creates the object, but does not instantiate visuals.
    // Those can be called as needed by the CreateVisual() function
    // Constructor for an enemy with health
    public Enemy(string name,
        GameObject prefab,
        int baseAttack,
        int baseAttackTimes,
        int baseMaxLife,
        int baseLife) : base(name, baseAttack, baseAttackTimes, true, baseMaxLife, baseLife) {
        this.prefab = prefab;
    }

    // Enemy without health
    public Enemy(string name,
        GameObject prefab,
        int baseAttack,
        int baseAttackTimes) : base(name, baseAttack, baseAttackTimes) {
        this.prefab = prefab;
    }

    public void CreateVisual() {
        // Spawn an object to view the summon on screen
        // Not using the ObjectPooler as there is only one player character
        GameObject enemyVisual = ObjectPooler.Spawn(prefab, new Vector3(0, 0, -10), Quaternion.identity);
        display = new EnemyView();
        display.InitializeView(enemyVisual, id);
        UpdateVisual();
    }

    // Function to call before moving object off the screen to another location (such as deck or discard)
    public void ClearVisual() {
        if (display != null) {
            display.Despawn();
            display = null;
        }
    }

    public void UpdateVisual() {
        display.SetActive(false);
        display.SetAttack(AttackValue);
        display.SetAttackTimes(AttackTimes);
        display.SetMaxLife(MaxLife);
        display.SetLife(LifeValue, MaxLife);
        display.SetActive(true);
    }

    public override void PerformAttacks() {
        // Resolve attacks on the front summon or player until it is dead, then continue until all attacks are gone or the player is defeated
        for (int i = 0; i < AttackTimes; i++) {
            // If there is a summon available to be hit by an attack, attack it
            if (!SummonController.SharedInstance.ReceiveAttack(this)) {
                // Otherwise, attack the player
                if (!PlayerController.SharedInstance.ReceiveAttack(this)) {
                    // If there are no entities to resolve attacks on (they're dead) escape the loop
                    break;
                }
            }
        }
    }
}
