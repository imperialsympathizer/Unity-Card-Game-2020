using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour {
    // This class is for initializing the game's framework and resources at startup
    // Currently it doesn't load other monobehaviour controllers (they are attached to the GameController in Unity),
    // But that may be something to do at a later date
    // This class is also used for generating unique ids of objects instantiated in the game

    private VisualController visualController;
    private GameEndManager gameEndManager;

    private static int uniqueId = 0;

    public static bool Loaded { get; private set; }

    private void Awake() {
        Loaded = false;
        InitializeResources();
    }

    private void InitializeResources() {
        // Create Controllers and Managers
        visualController = new VisualController(); // Dependencies - prefabs
        gameEndManager = new GameEndManager();

        // Load requisite prefabs into a dictionary indexed by prefab name
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs");
        Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

        for (int i = 0; i < prefabs.Length; i++) {
            prefabDictionary.Add(prefabs[i].name, prefabs[i]);
        }

        // Do the same for sprite images
        Sprite[] images = Resources.LoadAll<Sprite>("Images");
        Dictionary<string, Sprite> spriteDictionary = new Dictionary<string, Sprite>();

        for (int i = 0; i < images.Length; i++) {
            spriteDictionary.Add(images[i].name, images[i]);
        }
        // Initialize Controllers
        visualController.Initialize(prefabDictionary, spriteDictionary);
        NumberAnimator.Initialize();
        CardManager.SharedInstance.Initialize();
        PlayerController.Initialize();
        SummonController.Initialize();
        EnemyController.Initialize();
        gameEndManager.Initialize();
        // TargetSelector.Initialize();
        StaticEffectController.Initialize();
        AttackAnimator.Initialize();
        ElementController.Initialize();

        // When resources are completely loaded, sets the field that tells the TurnSystem to begin the game
        Loaded = true;
    }

    public static int GenerateId() {
        return uniqueId++;
    }
}
