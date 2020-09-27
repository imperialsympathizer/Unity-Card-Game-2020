using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Player : Fighter {
    // Class that houses the data for players
    // Contains references to PlayerView but does not directly control it in most instances
    // Accessed and instantiated through the PlayerController

    private string name;
    public int Id { get; private set; }
    public int MaxWill { get; private set; }
    public int WillValue { get; set; }

    public bool HasSlots { get; private set; }
    public int MaxSlots { get; private set; }
    public int SlotsValue { get; set; }

    private string description;

    // Visual component of the player, stored within its own View class
    private PlayerView display;
    private GameObject prefab;

    // Visual component of the slots
    private GameObject slotPrefab;

    // Since there probably won't be many player characters, we'll use an enum for setup
    public enum PlayerCharacter {
        NECROMANCER 
    }


    // Constructor that creates the object, but does not instantiate visuals.
    // Those can be called as needed by the CreateVisual() function
    public Player(PlayerCharacter player, int id) {
        switch (player) {
            case PlayerCharacter.NECROMANCER:
                this.name = "The Necromancer";
                this.description = "Don't let him near your graveyards.";
                this.Id = id;
                this.HasLife = true;
                this.MaxLife = 20;
                this.LifeValue = MaxLife;
                this.MaxWill = 20;
                this.WillValue = MaxWill;
                this.HasSlots = true;
                this.MaxSlots = 8;
                this.SlotsValue = 3;
                this.AttackValue = 5;
                this.AttackTimes = 1;
                this.prefab = VisualController.SharedInstance.GetPrefab("NecromancerPrefab");
                this.slotPrefab = VisualController.SharedInstance.GetPrefab("SlotPrefab");
                break;
        }
    }

    public void CreateVisual() {
        // Spawn an object to view the player on screen
        // Not using the ObjectPooler as there is only one player character
        GameObject playerVisual = GameObject.Instantiate(prefab, new Vector3(0, 0, -10), Quaternion.identity);
        display = new PlayerView();
        display.InitializeView(playerVisual, Id, slotPrefab, SlotsValue);
        UpdateVisual();
    }

    public void AddSlot() {
        if (SlotsValue < MaxSlots) {
            SlotsValue++;
            display.AddSlot();
        }
    }

    public void RemoveSlot() {
        if (SlotsValue > 0) {
            SlotsValue--;
            display.RemoveSlot();
        }
    }

    // Function to call before moving card off the screen to another location (such as deck or discard)
    public void ClearVisual() {
        if (display != null) {
            // Destroy(display);
            display = null;
        }
    }

    public void UpdateVisual() {
        display.SetActive(false);
        display.SetAttack(AttackValue);
        display.SetAttackTimes(AttackTimes);
        display.SetLife(LifeValue);
        display.SetWill(WillValue);
        display.SetActive(true);
    }
}
