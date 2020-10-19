using System.Collections.Generic;

public static class EnemyController {
    // Dictionary is used for searching for a specific summon by Id (when targeting, etc.)
    // List is used for resolving things like combat, where iteration is important
    private static Dictionary<int, Enemy> enemyDictionary = new Dictionary<int, Enemy>();

    public static void Initialize() {
        CreateEnemy(Enemy.EnemyType.KNIGHT);
    }

    public static void CreateEnemy(Enemy.EnemyType enemyType) {
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
    }

    public static void UpdateLife(int enemyId, int val) {
        Enemy enemy = GetEnemy(enemyId);
        if (enemy != null) {
            enemy.UpdateLifeValue(val);
            if (enemy.CheckDeath()) {
                enemy.ClearVisual();
                enemyDictionary.Remove(enemyId);
            }
            else {
                enemyDictionary[enemyId] = enemy;
            }
        }
    }

    public static Enemy GetDefender() {
        // This function returns null if there are no enemies available to take damage from an attack
        if (enemyDictionary.Count < 1) {
            return null;
        }

        // Only attack the enemy if it has life
        foreach (KeyValuePair<int, Enemy> enemyEntry in enemyDictionary) {
            if (enemyEntry.Value.HasLife) {
                return enemyEntry.Value;
            }
        }

        return null;
    }

    public static Enemy GetEnemy(int enemyId) {
        // This function returns null if the requested enemy does not exist
        if (enemyDictionary.ContainsKey(enemyId)) {
            return enemyDictionary[enemyId];
        }

        return null;
    }

    public static bool CompleteAttack(int enemyId, Fighter attacker) {
        // Change the life total to reflect damage taken
        Enemy defender = enemyDictionary[enemyId];
        if (defender != null) {
            defender.ReceiveAttack(attacker);

            // If damage exceeds the life remaining, the summon is defeated
            // Otherwise, update the life total and visuals
            if (defender.LifeValue <= 0) {
                // TODO: death animation
                // Clear the visual first to ensure proper removal
                defender.ClearVisual();
                enemyDictionary.Remove(enemyId);
            }
            else {
                defender.UpdateVisual();
                // Update the enemy objects in the list and dictionary
                enemyDictionary[defender.id] = defender;
            }

            return true;
        }

        return false;
    }

    public static List<Enemy> GetEnemyList() {
        List<Enemy> enemies = new List<Enemy>();
        foreach (KeyValuePair<int, Enemy> enemyEntry in enemyDictionary) {
            if (enemyEntry.Value != null) {
                enemies.Add(enemyEntry.Value);
            }
        }
        return enemies;
    }

    public static Enemy GetRandomEnemy(bool hasLife = false) {
        // This function returns null if there are no enemies available
        if (enemyDictionary.Count < 1) {
            return null;
        }

        List<Enemy> enemiesWithLife = GetEnemyList();

        if (hasLife) {
            foreach (Enemy enemy in enemiesWithLife) {
                if (!enemy.HasLife) {
                    enemiesWithLife.Remove(enemy);
                }
            }
        }

        return enemiesWithLife[RandomNumberGenerator.getRandomIndexFromRange(enemiesWithLife.Count - 1)];
    }

    private static void UpdateVisual(int id) {
        // Updates any visuals that display enemy data
        if (enemyDictionary.TryGetValue(id, out Enemy editEnemy)) {
            editEnemy.UpdateVisual();
        }
    }
}
