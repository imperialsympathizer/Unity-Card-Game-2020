public static class PlayerController {

    private static Player player;

    public static void Initialize() {
        CreatePlayer(Player.PlayerCharacter.NECROMANCER);
    }

    public static void CreatePlayer(Player.PlayerCharacter character) {
        switch (character) {
            case Player.PlayerCharacter.NECROMANCER:
                player = new Player("The Necromancer", VisualController.SharedInstance.GetPrefab("NecromancerPrefab"), 5, 1, 20, 20, 20, 20, true, 8, 3);
                break;
        }
        player.CreateVisual();
    }

    public static int GetSlotsValue() {
        return player.SlotsValue;
    }

    public static void AddSlot() {
        player.AddSlot();
    }

    public static void RemoveSlot() {
        player.RemoveSlot();
    }

    public static int GetLife() {
        return player.LifeValue;
    }

    public static int GetWill() {
        return player.WillValue;
    }

    public static void UpdateLife(int val) {
        player.UpdateLifeValue(val);
        player.CheckDeath();
    }

    public static void UpdateWill(int val) {
        player.UpdateWillValue(val);
        player.CheckDeath();
    }

    public static void UpdateVisual() {
        // Updates any visuals that display player data
        player.UpdateVisual();
    }

    public static Player GetPlayer() {
        return player;
    }

    public static bool CompleteAttack(Fighter attacker) {
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
