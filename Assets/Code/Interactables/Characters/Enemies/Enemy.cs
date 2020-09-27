using System;
using UnityEngine;

public class Enemy : Fighter {
    // Class that houses the data for nemies
    // Contains references to EnemyView but does not directly control it in most instances
    // Accessed and instantiated through the EnemyController

    private string name;
    public int Id { get; private set; }

    private string description;

    // Visual component of the player, stored within its own View class
    private EnemyView display;
    private GameObject prefab;

    public enum EnemyType {
        KNIGHT
    }


    // Constructor that creates the object, but does not instantiate visuals.
    // Those can be called as needed by the CreateVisual() function
    public Enemy(EnemyType enemyType, int id) {
        switch (enemyType) {
            case EnemyType.KNIGHT:
                this.name = "Knight";
                this.description = "You'll have to get through him first";
                this.Id = id;
                this.HasLife = true;
                this.MaxLife = 50;
                this.LifeValue = MaxLife;
                this.AttackValue = 10;
                this.AttackTimes = 2;
                this.prefab = VisualController.SharedInstance.GetPrefab("KnightPrefab");
                break;
        }
    }

    public void CreateVisual() {
        // Spawn an object to view the summon on screen
        // Not using the ObjectPooler as there is only one player character
        GameObject enemyVisual = ObjectPooler.Spawn(prefab, new Vector3(0, 0, -10), Quaternion.identity);
        display = new EnemyView();
        display.InitializeView(enemyVisual, Id);
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
        display.SetLife(LifeValue);
        display.SetActive(true);
    }
}
