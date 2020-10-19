using UnityEngine;

public class Enemy : Fighter {
    // Class that houses the data for nemies
    // Contains references to EnemyView but does not directly control it in most instances
    // Accessed and instantiated through the EnemyController
    private EnemyView display;

    // Visual component of the player, stored within its own View class
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
        int baseLife) : base(name, FighterType.ENEMY, baseAttack, baseAttackTimes, true, baseMaxLife, baseLife) {
        this.prefab = prefab;
    }

    // Enemy without health
    public Enemy(string name,
        GameObject prefab,
        int baseAttack,
        int baseAttackTimes) : base(name, FighterType.ENEMY, baseAttack, baseAttackTimes) {
        this.prefab = prefab;
    }

    public override void CreateVisual() {
        // Spawn an object to view the summon on screen
        // Not using the ObjectPooler as there is only one player character
        GameObject enemyVisual = ObjectPooler.Spawn(prefab, new Vector3(0, 0, -10), Quaternion.identity);
        display = new EnemyView(enemyVisual, id, 410);
        UpdateVisual();
    }

    public override void UpdateVisual() {
        display.SetActive(false);
        display.SetAttack(AttackValue);
        display.SetAttackTimes(AttackValue, AttackTimes);
        display.SetMaxLife(MaxLife);
        display.SetLife(HasLife, LifeValue, MaxLife);
        display.SetActive(true);
    }

    public override void EnableVisual(bool enable) {
        display.SetActive(enable);
    }

    public override void ClearVisual() {
        if (display != null) {
            display.Despawn();
            display = null;
        }
    }

    public override RectTransform GetVisualRect() {
        return display.getVisualRect();
    }

    public override void SetVisualOutline(Color color) {
        display.SetVisualOutline(color);
    }

    public override void PerformAttack() {
        display.AnimateAttack();
    }


}
