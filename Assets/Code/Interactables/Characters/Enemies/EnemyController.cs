using System.Collections.Generic;

public class EnemyController : BaseController {
    public static EnemyController Instance;
    // Dictionary is used for searching for a specific summon by Id (when targeting, etc.)
    // List is used for resolving things like combat, where iteration is important
    private Dictionary<int, Enemy> enemyDictionary = new Dictionary<int, Enemy>();

    protected override bool Initialize() {
        Instance = this;
        if (VisualController.Instance != null && VisualController.Instance.Initialized &&
            NumberAnimator.Instance != null && NumberAnimator.Instance.Initialized) {
            CreateEnemy(Enemy.EnemyType.KNIGHT);

            return true;
        }

        return false;
    }

    public void CreateEnemy(Enemy.EnemyType enemyType) {
        Enemy newEnemy;
        switch (enemyType) {
            case Enemy.EnemyType.KNIGHT:
            default:
                newEnemy = new Enemy("Knight", VisualController.Instance.GetPrefab("KnightPrefab"), 10, 2, 50, 50);
                break;
        }
        newEnemy.CreateVisual();
        enemyDictionary.Add(newEnemy.id, newEnemy);
    }

    #region Update
    // Only updaters necessary are for MaxLife and LifeValue to check external conditions after update
    public void UpdateMaxLife(int enemyId, int val) {
        Enemy enemy = GetEnemy(enemyId);
        if (enemy != null) {
            if (enemy.UpdateMaxLife(val)) {
                enemy.ClearVisual();
                enemyDictionary.Remove(enemyId);
            }
        }
    }

    public void UpdateLife(int enemyId, int val) {
        Enemy enemy = GetEnemy(enemyId);
        if (enemy != null) {
            // If the enemy dies from this change, remove it
            if (enemy.UpdateLifeValue(val)) {
                enemy.ClearVisual();
                enemyDictionary.Remove(enemyId);
            }
        }
    }
    #endregion

    #region Getters
    public Enemy GetEnemy(int enemyId) {
        // This function returns null if the requested enemy does not exist
        if (enemyDictionary.TryGetValue(enemyId, out Enemy enemy)) {
            return enemy;
        }

        return null;
    }

    public Enemy GetDefender() {
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

    public List<Enemy> GetEnemyList() {
        List<Enemy> enemies = new List<Enemy>();
        foreach (KeyValuePair<int, Enemy> enemyEntry in enemyDictionary) {
            if (enemyEntry.Value != null) {
                enemies.Add(enemyEntry.Value);
            }
        }
        return enemies;
    }

    public Enemy GetRandomEnemy(bool hasLife = false) {
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
    #endregion

    public bool CompleteAttack(int enemyId, Fighter attacker) {
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
}
