using System.Collections.Generic;
using UnityEngine;

public class EnemyController {
    public static EnemyController SharedInstance;

    // Dictionary is used for searching for a specific summon by Id (when targeting, etc.)
    // List is used for resolving things like combat, where iteration is important
    private Dictionary<int, Enemy> enemyDictionary = new Dictionary<int, Enemy>();
    private List<Enemy> enemyList = new List<Enemy>();

    public void Initialize() {
        SharedInstance = this;
        Enemy newEnemy = new Enemy(Enemy.EnemyType.KNIGHT, ResourceController.GenerateId());
        enemyDictionary.Add(newEnemy.id, newEnemy);
        enemyList.Add(newEnemy);
        newEnemy.CreateVisual();
    }

    public void CreateEnemy(Enemy.EnemyType enemyType) {
        Enemy newEnemy = new Enemy(enemyType, ResourceController.GenerateId());
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

        // Change the life total to reflect damage taken
        Enemy enemy = enemyList[0];
        int lifeResult = enemy.LifeValue - damage;

        // If damage exceeds the life remaining, the summon is defeated
        // Otherwise, update the life total and visuals
        if (lifeResult <= 0) {
            // TODO: death animation
            // Clear the visual first to ensure proper removal
            enemy.ClearVisual();
            enemyList.RemoveAt(0);
            enemyDictionary.Remove(enemy.id);
        }
        else {
            enemy.LifeValue = lifeResult;
            // Update the summon objects in the list and dictionary
            enemyList[0] = enemy;
            enemyDictionary[enemy.id] = enemy;
            // Update the list
            enemy.UpdateVisual();
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
        // Updates any visuals that display player data
        Enemy editEnemy;
        if (enemyDictionary.TryGetValue(id, out editEnemy)) {
            editEnemy.UpdateVisual();
        }
    }
}
