using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour {
    // This class is for initializing the game's framework and resources at startup
    // It is also used for generating unique ids of objects instantiated in the game

    private VisualController visualController = new VisualController();
    private NumberAnimator numberAnimator = new NumberAnimator();
    private PlayerController playerController = new PlayerController();
    private SummonController summonController = new SummonController();
    private EnemyController enemyController = new EnemyController();
    private GameEndManager gameEndManager = new GameEndManager();
    private StaticEffectController staticEffectController = new StaticEffectController();
    private AttackAnimator attackAnimator = new AttackAnimator();
    private ElementController elementController = new ElementController();
    private ArtifactController artifactController = new ArtifactController();

    private static int uniqueId = 0;

    public static Dictionary<string, GameObject> prefabDictionary;
    public static Dictionary<string, Sprite> spriteDictionary;

    public static bool Loaded { get; private set; } = false;

    private void Awake() {
        InitializeResources();
    }

    private void InitializeResources() {
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

        // Initialize Controllers asynchronously through coroutines
        StartCoroutine(LoadController(visualController));
        StartCoroutine(CardManager.Instance.Initialize());
        StartCoroutine(LoadController(elementController));
        StartCoroutine(LoadController(artifactController));
        StartCoroutine(LoadController(playerController));
        StartCoroutine(LoadController(summonController));
        StartCoroutine(LoadController(enemyController));
        StartCoroutine(LoadController(staticEffectController));
        StartCoroutine(LoadController(gameEndManager));
        StartCoroutine(LoadController(numberAnimator));
        StartCoroutine(LoadController(attackAnimator));

        // When resources are completely loaded, tell the TurnSystem to begin the game
        StartCoroutine(WaitForLoadingComplete());
    }

    private IEnumerator LoadController(BaseController controller) {
        while (!controller.InitializeController()) {
            yield return null;
        }
    }

    private IEnumerator WaitForLoadingComplete() {
        while (!visualController.Initialized ||
            !elementController.Initialized ||
            !artifactController.Initialized ||
            !playerController.Initialized ||
            !summonController.Initialized ||
            !enemyController.Initialized ||
            !staticEffectController.Initialized ||
            !gameEndManager.Initialized ||
            !numberAnimator.Initialized ||
            !attackAnimator.Initialized ||
            !CardManager.Instance.Initialized) {
            yield return null;
        }

        Loaded = true;
    }

    public static int GenerateId() {
        return uniqueId++;
    }
}
