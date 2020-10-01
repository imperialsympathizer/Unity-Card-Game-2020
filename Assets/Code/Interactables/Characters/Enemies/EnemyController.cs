using System.Collections.Generic;

public static class EnemyController {

    // Dictionary is used for searching for a specific summon by Id (when targeting, etc.)
    // List is used for resolving things like combat, where iteration is important
    private static Dictionary<int, Enemy> enemyDictionary = new Dictionary<int, Enemy>();
    private static List<Enemy> enemyList = new List<Enemy>();

    // Cache the index of an enemy whenever it is attacked
    private static int index;

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
        enemyList.Add(newEnemy);
    }

    public static Enemy GetDefender() {
        // This function returns null if there are no enemies available to take damage from an attack
        index = 0;
        if (enemyList.Count < 1) {
            return null;
        }

        // Only attack the enemy if it has life
        Enemy defender = enemyList[index];
        while (!defender.HasLife) {
            index++;
            if (index >= enemyList.Count) {
                return null;
            }

            defender = enemyList[index];
        }

        return defender;
    }

    public static bool CompleteAttack(int enemyId, Fighter attacker) {
        // Change the life total to reflect damage taken
        Enemy defender = enemyDictionary[enemyId];
        defender.ReceiveAttack(attacker);

        // If damage exceeds the life remaining, the summon is defeated
        // Otherwise, update the life total and visuals
        if (defender.LifeValue <= 0) {
            // TODO: death animation
            // Clear the visual first to ensure proper removal
            defender.ClearVisual();
            enemyList.RemoveAt(0);
            enemyDictionary.Remove(defender.id);
        }
        else {
            defender.UpdateVisual();
            // Update the enemy objects in the list and dictionary
            enemyList[index] = defender;
            enemyDictionary[defender.id] = defender;
        }

        return true;
    }

    public static List<Enemy> GetEnemyList() {
        return enemyList;
    }

    private static void UpdateVisual(int id) {
        // Updates any visuals that display enemy data
        Enemy editEnemy;
        if (enemyDictionary.TryGetValue(id, out editEnemy)) {
            editEnemy.UpdateVisual();
        }
    }
}
