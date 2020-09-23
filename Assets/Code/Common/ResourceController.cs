using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceController : MonoBehaviour {
    // This class is for initializing the game's framework at startup
    // Currently it doesn't load other monobehaviour controllers (they are attached to the GameController in Unity),
    // But that may be something to do at a later date
    // This class is also used for generating ids of objects spawned in the game

    // May not want to give access to the resource controller for general use (GenerateId is already static)
    // TODO: remove
    public static ResourceController SharedInstance;

    private VisualController visualController;
    private PlayerController playerController;
    private SummonController summonController;
    private EnemyController enemyController;

    private NumberAnimator numberAnimator;
    private CardManager cardManager;
    private GameEndManager gameOverManager;

    private static int uniqueId = 0;

    public static bool Loaded { get; private set; }

    private void Awake() {
        SharedInstance = this;
        Loaded = false;
        InitializeResources();
    }

    private void InitializeResources() {
        Debug.Log("Loading resources");

        // Create Controllers and Managers
        visualController = new VisualController(); // Dependencies - prefabs
        cardManager = new CardManager(); // Dependencies - VisualController (prefabs)
        playerController = new PlayerController(); // Dependencies - VisualController (prefabs), NumberAnimator, needs to know what player to spawn
        summonController = new SummonController(); // Dependencies - VisualController
        enemyController = new EnemyController();

        gameOverManager = new GameEndManager();
        numberAnimator = new NumberAnimator();

        // Load requisite resources
        GameObject cardPrefab = Resources.Load<GameObject>("Prefabs/Card");
        if (cardPrefab == null)
            Debug.Log("failed to load prefab.");

        GameObject necromancerPrefab = Resources.Load<GameObject>("Prefabs/Necromancer Sprite");
        if (necromancerPrefab == null)
            Debug.Log("failed to load prefab.");

        GameObject slotPrefab = Resources.Load<GameObject>("Prefabs/Slot Sprite");
        if (slotPrefab == null)
            Debug.Log("failed to load prefab.");

        GameObject zombiePrefab = Resources.Load<GameObject>("Prefabs/Zombie Sprite");
        if (zombiePrefab == null)
            Debug.Log("failed to load prefab.");

        GameObject knightPrefab = Resources.Load<GameObject>("Prefabs/Knight Sprite");
        if (knightPrefab == null)
            Debug.Log("failed to load prefab.");

        // Initialize Controllers
        visualController.Initialize(cardPrefab, necromancerPrefab, slotPrefab, zombiePrefab, knightPrefab);
        numberAnimator.Initialize();
        cardManager.Initialize();
        playerController.Initialize();
        summonController.Initialize();
        enemyController.Initialize();
        gameOverManager.Initialize();

        // When resources are completely loaded, fires an event that tells the TurnSystem to begin the game
        Loaded = true;
    }

    public static int GenerateId() {
        return uniqueId++;
    }
}
