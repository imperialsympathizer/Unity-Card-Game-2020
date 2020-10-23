public class PlayerController : BaseController {
    public static PlayerController Instance;

    private Player player;

    protected override bool Initialize() {
        Instance = this;
        if (VisualController.Instance != null && VisualController.Instance.Initialized &&
            NumberAnimator.Instance != null && NumberAnimator.Instance.Initialized) {
            CreatePlayer(Player.PlayerCharacter.NECROMANCER);

            return true;
        }

        return false;
    }

    public void CreatePlayer(Player.PlayerCharacter character) {
        switch (character) {
            case Player.PlayerCharacter.NECROMANCER:
            default:
                player = new Player("The Necromancer", VisualController.Instance.GetPrefab("NecromancerPrefab"), 5, 1, 10, 10, 20, 20, true, 8, 3);
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

    public int GetVigor() {
        return player.VigorValue;
    }

    public void UpdateLife(int val) {
        if (player.UpdateLifeValue(val)) {
            TurnSystem.Instance.CheckGameConditions();
        }
    }

    public void UpdateWill(int val) {
        player.UpdateWillValue(val);
        if (player.CheckDeath()) {
            TurnSystem.Instance.CheckGameConditions();
        }
    }

    public void UpdateVigor(int val) {
        player.UpdateVigorValue(val);
    }

    public void ResetVigor() {
        player.UpdateVigorValue(player.MaxLife - player.VigorValue);
    }

    public void UpdateVisual() {
        // Updates any visuals that display player data
        player.UpdateVisual();
    }

    public Player GetPlayer() {
        return player;
    }

    public bool CompleteAttack(Fighter attacker) {
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
