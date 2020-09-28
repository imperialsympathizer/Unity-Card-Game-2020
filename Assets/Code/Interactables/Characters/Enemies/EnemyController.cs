using System.Collections.Generic;

public class EnemyController {
    public static EnemyController SharedInstance;

    // Dictionary is used for searching for a specific summon by Id (when targeting, etc.)
    // List is used for resolving things like combat, where iteration is important
    private Dictionary<int, Enemy> enemyDictionary = new Dictionary<int, Enemy>();
    private List<Enemy> enemyList = new List<Enemy>();

    public void Initialize() {
        SharedInstance = this;
        CreateEnemy(Enemy.EnemyType.KNIGHT);
    }

    public void CreateEnemy(Enemy.EnemyType enemyType) {
        Enemy newEnemy;
        switch (enemyType) {
            case Enemy.EnemyType.KNIGHT:
                newEnemy = new Enemy("Knight", VisualController.SharedInstance.GetPrefab("KnightPrefab"), 10, 2, 50, 50);
                break;
            default:
                // Default enemy is knight
                newEnemy = new Enemy("Knight", VisualController.SharedInstance.GetPrefab("KnightPrefab"), 10, 2, 50, 50);
                break;
        }
        newEnemy.CreateVisual();
        enemyDictionary.Add(newEnemy.id, newEnemy);
        enemyList.Add(newEnemy);
    }

    public bool ResolveAttack(int damage) {
        // This function returns false if there are no enemies available to take damage from an attack
        // Otherwise, damage is dealt to the front enemy (at the beginning of the list)
        if (enemyList.Count < 1) {
            return false;
        }
        
        // Only attack the enemy if it has life
        // Otherwise, attack the next enemy
        int enemyIndex = 0;
        Enemy enemy = enemyList[enemyIndex];
        while (!enemy.HasLife) {
            enemyIndex++;
            if (enemyIndex >= enemyList.Count) {
                return false;
            }

            enemy = enemyList[enemyIndex];
        }

        // Change the life total to reflect damage taken
        enemy.UpdateLifeValue(-damage);

        // If damage exceeds the life remaining, the summon is defeated
        // Otherwise, update the life total and visuals
        if (enemy.LifeValue <= 0) {
            // TODO: death animation
            // Clear the visual first to ensure proper removal
            enemy.ClearVisual();
            enemyList.RemoveAt(0);
            enemyDictionary.Remove(enemy.id);
        }
        else {
            enemy.UpdateVisual();
            // Update the enemy objects in the list and dictionary
            enemyList[enemyIndex] = enemy;
            enemyDictionary[enemy.id] = enemy;
        }

        return true;
    }

    public Queue<int> GetAttackQueue() {
        // Returns a queue (FIFO) of attacks performed by summons
        // Each summon will add its attackValue to a new entry in the queue as many times as their attackTimes is
        Queue<int> attacks = new Queue<int>();
        for (int i = 0; i < enemyList.Count; i++) {
            Enemy enemy = enemyList[i];
            if (enemy.AttackTimes > 0) {
                for (int j = 0; j < enemy.AttackTimes; j++) {
                    attacks.Enqueue(enemy.AttackValue);
                }
            }
        }

        return attacks;
    }

    public List<Enemy> GetEnemyList() {
        return enemyList;
    }

    private void UpdateVisual(int id) {
        // Updates any visuals that display enemy data
        Enemy editEnemy;
        if (enemyDictionary.TryGetValue(id, out editEnemy)) {
            editEnemy.UpdateVisual();
        }
    }
}
