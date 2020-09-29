﻿using System.Collections.Generic;

public class PlayerController {
    public static PlayerController SharedInstance;

    private Player player;

    public void Initialize() {
        SharedInstance = this;
        CreatePlayer(Player.PlayerCharacter.NECROMANCER);
    }

    public void CreatePlayer(Player.PlayerCharacter character) {
        switch (character) {
            case Player.PlayerCharacter.NECROMANCER:
                player = new Player("The Necromancer", VisualController.SharedInstance.GetPrefab("NecromancerPrefab"), 5, 1, 20, 20, 20, 20, true, 8, 3);
                break;
        }
        player.CreateVisual();
    }

    public int GetSlotsValue() {
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

    public void UpdateLife(int val) {
        player.UpdateLifeValue(val);
    }

    public void UpdateWill(int val) {
        player.UpdateWillValue(val);
    }

    public void UpdateVisual() {
        // Updates any visuals that display player data
        player.UpdateVisual();
    }

    public void PerformAttacks() {
        player.PerformAttacks();
    }

    public bool ReceiveAttack(Attacker attacker) {
        // This function returns false if the player is already considered dead
        if (player.LifeValue < 1 && player.WillValue < 1) {
            return false;
        }

        // Change the life and will totals to reflect damage taken
        player.ReceiveAttack(attacker);

        // If damage exceeds the life remaining, the player is defeated
        // In both cases, update the life, will and visuals
        if (player.LifeValue < 1 && player.WillValue < 1) {
            // TODO: death animation
        }

        player.UpdateVisual();

        return true;
    }
}
