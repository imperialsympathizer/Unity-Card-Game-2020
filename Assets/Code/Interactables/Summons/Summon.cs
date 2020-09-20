﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Summon : Fighter {
    // Class that houses the data for summons
    // Contains references to SummonView but does not directly control it in most instances
    // Accessed and instantiated through the SummonController

    private string name;
    public int Id { get; private set; }

    private string description;

    // Visual component of the player, stored within its own View class
    private SummonView display;
    private GameObject prefab;

    public enum Summonable {
        ZOMBIE
    }


    // Constructor that creates the object, but does not instantiate visuals.
    // Those can be called as needed by the CreateVisual() function
    public Summon(Summonable summonType, int id) {
        switch (summonType) {
            case Summonable.ZOMBIE:
                this.name = "Zombie";
                this.description = "Not much more than rotten meat";
                this.Id = id;
                this.HasLife = true;
                this.MaxLife = 5;
                this.LifeValue = MaxLife;
                this.AttackValue = 5;
                this.AttackTimes = 1;
                this.prefab = VisualController.SharedInstance.zombiePrefab;
                break;
        }
    }

    public void CreateVisual() {
        // Spawn an object to view the summon on screen
        // Not using the ObjectPooler as there is only one player character
        GameObject summonVisual = ObjectPooler.Spawn(prefab, new Vector3(0, 0, -10), Quaternion.identity);
        display = new SummonView();
        display.InitializeView(summonVisual, Id);
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