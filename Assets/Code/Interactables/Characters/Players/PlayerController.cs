using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController {
    public static PlayerController SharedInstance;

    private Player player;

    // These canvases contain visuals relevant to the player's stats
    private GameObject playerCanvas;
    private GameObject slotCanvas;

    public void Initialize() {
        SharedInstance = this;
        player = new Player(Player.PlayerCharacter.NECROMANCER, ResourceController.GenerateId());
        player.CreateVisual();
    }

    public int GetAvailableSlots() {
        return player.SlotsValue;
    }

    public void AddSlot() {
        player.AddSlot();
    }

    public void RemoveSlot() {
        player.RemoveSlot();
    }

    public int GetLife() {
        return player.LifeValue;
    }

    public int GetWill() {
        return player.WillValue;
    }

    public void SetLife(int val) {
        if (player.HasLife) {
            player.LifeValue = val;
        }
    }

    public void SetWill(int val) {
        if (player.HasLife) {
            player.WillValue = val;
        }
    }

    public void UpdateVisual() {
        // Updates any visuals that display player data
        player.UpdateVisual();
    }

    public bool ResolveAttack(int damage) {
        // This function returns false if the player is already considered dead
        if (player.LifeValue < 1 && player.WillValue < 1) {
            return false;
        }

        // Change the life and will totals to reflect damage taken
        int lifeResult = player.LifeValue - damage;
        int willResult = player.WillValue;
        while (lifeResult < 1) {
            lifeResult += 20;
            willResult--;
        }

        // If damage exceeds the life remaining, the player is defeated
        // In both cases, update the life, will and visuals
        if (lifeResult < 1 && willResult < 1) {
            // TODO: death animation
        }

        player.LifeValue = lifeResult;
        player.WillValue = willResult;
        player.UpdateVisual();

        return true;
    }

    public Queue<int> GetAttackQueue() {
        // Returns a queue (FIFO) of attacks performed by the player
        Queue<int> attacks = new Queue<int>();
        if (player.AttackTimes > 0) {
            for (int j = 0; j < player.AttackTimes; j++) {
                attacks.Enqueue(player.AttackValue);
            }
        }

        return attacks;
    }
}
