using System.Collections.Generic;
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
    private TargetSelector targetSelector;
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
        targetSelector = new TargetSelector();

        // Load requisite prefabs into a dictionary indexed by prefab name
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs");
        Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

        for (int i = 0; i < prefabs.Length; i++) {
            prefabDictionary.Add(prefabs[i].name, prefabs[i]);
        }

        // Initialize Controllers
        visualController.Initialize(prefabDictionary);
        numberAnimator.Initialize();
        cardManager.Initialize();
        playerController.Initialize();
        summonController.Initialize();
        enemyController.Initialize();
        gameOverManager.Initialize();
        targetSelector.Initialize();

        // When resources are completely loaded, fires an event that tells the TurnSystem to begin the game
        Loaded = true;
    }

    public static int GenerateId() {
        return uniqueId++;
    }
}
