using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour {
    // This class is for initializing the game's framework and resources at startup
    // It is also used for generating unique ids of objects instantiated in the game
    public static ResourceController Instance;

    private List<BaseController> controllers;

    private static int uniqueId = 0;

    public static Dictionary<string, GameObject> prefabDictionary;
    public static Dictionary<string, Sprite> spriteDictionary;

    public static Player playerCharacter;
    public static Deck runDeck;
    public static List<Artifact> runArtifacts;

    public static bool Loaded { get; private set; } = false;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this) {
            // When new scenes are loaded, a new GameController object will be created
            // But we don't need it because the old GameController will persist, so the new controller should be destroyed
            Destroy(this.gameObject);
        }
    }

    public void Load(bool reinitialize) {
        Loaded = false;
        if (Instance == this) {
            // Instead of doing a full reload of resources, reinitialize them with the required data
            InitializeResources(reinitialize);
        }
    }

    private void InitializeResources(bool reinitialize) {
        Loaded = false;
        // These resources only need to be loaded once
        if (!reinitialize) {
            // Load requisite prefabs into a dictionary indexed by prefab name
            GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs");
            prefabDictionary = new Dictionary<string, GameObject>();

            for (int i = 0; i < prefabs.Length; i++) {
                prefabDictionary.Add(prefabs[i].name, prefabs[i]);
            }

            // Do the same for sprite images
            Sprite[] images = Resources.LoadAll<Sprite>("Images");
            spriteDictionary = new Dictionary<string, Sprite>();

            for (int i = 0; i < images.Length; i++) {
                spriteDictionary.Add(images[i].name, images[i]);
            }

            controllers = new List<BaseController> {
                new VisualController(),
                new NumberAnimator(),
                new PlayerController(),
                new SummonController(),
                new EnemyController(),
                new GameEndManager(),
                new StaticEffectController(),
                new AttackAnimator(),
                new ElementController(),
                new ArtifactController(),
                new RewardController()
            };
        }

        // Initialize/Reinitialize Controllers asynchronously through coroutines
        foreach (BaseController controller in controllers) {
            controller.Initialized = false;
        }
        CardManager.Instance.ResetInitialization();
        StartCoroutine(CardManager.Instance.Initialize(reinitialize));
        foreach (BaseController controller in controllers) {
            StartCoroutine(InitializeController(controller, reinitialize));
        }

        // When resources are completely loaded, tell the TurnSystem to begin the game
        StartCoroutine(WaitForLoadingComplete());
    }

    private IEnumerator InitializeController(BaseController controller, bool reinitialize = false) {
        while (!controller.InitializeController(reinitialize)) {
            yield return null;
        }
    }

    private IEnumerator WaitForLoadingComplete() {
        while (!Loaded) {
            bool status = true;
            foreach (BaseController controller in controllers) {
                if (!controller.Initialized) {
                    status = false;
                    break;
                }
            }
            if (!CardManager.Instance.Initialized) {
                status = false;
            }
            Loaded = status;
            yield return null;
        }
    }

    public static int GenerateId() {
        return uniqueId++;
    }

    public void DeloadLevel() {
        // Store all necessary resources first, then load the next level scene
        
        playerCharacter = PlayerController.Instance.GetPlayer();
        runDeck = CardManager.Instance.GetRunDeck();
        runArtifacts = ArtifactController.Instance.GetRunArtifacts();
        // Reset the unique Id counter and 
        uniqueId = 0;
        // Load the next level
        LevelLoader.Instance.LoadLevel(LevelLoader.Level.BATTLE);
    }
}
