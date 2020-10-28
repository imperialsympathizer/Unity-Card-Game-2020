using System.Collections;

public class TurnSystem : StateMachine {
    public static TurnSystem Instance;

    public static int turnCount = 0;

    public static bool battleOver = false;

    void Awake() {
        Instance = this;
        StartCoroutine(AwaitStart());
    }

    private IEnumerator AwaitStart() {
        // Ensure resources are loaded before starting the battle
        while (ResourceController.Instance == null) {
            yield return null;
        }

        bool reinitialize = ResourceController.Loaded;
        ResourceController.Instance.Load(reinitialize);

        while (!ResourceController.Loaded) {
            yield return null;
        }

        SetState(new BeginBattle(this));
    }

    public bool IsPlayerTurn() {
        return (State.GetType() == typeof(PlayerTurn));
    }

    public bool CheckGameConditions() {
        bool victory = false;
        if (!battleOver) {
            // This method checks for all win and loss conditions
            // If one is reached, sets the state to EndBattle
            // Otherwise, nothing happens

            // If the player has no will left, defeat
            if (PlayerController.Instance.GetWill() <= 0) {
                // Debug.Log("You Lose.");
                battleOver = true;
                victory = false;
            }

            // If all enemies are dead, win
            if (EnemyController.Instance.GetEnemyList().Count < 1) {
                // Debug.Log("You Win.");
                battleOver = true;
                victory = true;
            }

            // TODO: remove, this is being used for testing purposes
            //battleOver = true;
            //victory = true;
        }

        if (battleOver) {
            SetState(new EndBattle(this, victory));
        }
        return battleOver;
    }
}
