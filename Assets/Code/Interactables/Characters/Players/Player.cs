using UnityEngine;

public class Player : Fighter {
    // Class that houses the data for players
    // Contains references to PlayerView but does not directly control it in most instances
    // Accessed and instantiated through the PlayerController
    public int MaxWill { get; private set; }
    public int WillValue { get; private set; }

    public bool HasSlots { get; private set; }
    public int MaxSlots { get; private set; }
    public int SlotsValue { get; private set; }

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
    public Player(
        string name,
        GameObject prefab,
        int baseAttack,
        int baseAttackTimes,
        int baseMaxLife,
        int baseLife,
        int baseMaxWill, 
        int baseWillValue,
        bool hasSlots,
        int maxSlots = 0,
        int slotsValue = 0) : base(name, baseAttack, baseAttackTimes, true, baseMaxLife, baseLife) {
        this.prefab = prefab;
        slotPrefab = VisualController.SharedInstance.GetPrefab("SlotPrefab");
        MaxWill = baseMaxWill;
        WillValue = baseWillValue;
        HasSlots = hasSlots;
        MaxSlots = maxSlots;
        SlotsValue = slotsValue;
    }

    #region Visual Methods

    public void CreateVisual() {
        // Spawn an object to view the player on screen
        // Not using the ObjectPooler as there is only one player character
        GameObject playerVisual = GameObject.Instantiate(prefab, new Vector3(0, 0, -10), Quaternion.identity);
        display = new PlayerView();
        display.InitializeView(playerVisual, id, slotPrefab, SlotsValue);
        UpdateVisual();
    }

    public void ClearVisual() {
        if (display != null) {
            display.Despawn();
            display = null;
        }
    }

    public override void UpdateVisual() {
        display.SetActive(false);
        display.SetAttack(AttackValue);
        display.SetAttackTimes(AttackTimes);
        display.SetLife(LifeValue);
        display.SetWill(WillValue);
        display.SetActive(true);
    }

    #endregion

    #region Data Methods
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

    // Since the player's life is tied to their will, need to override the Fighter update method
    public new void UpdateLifeValue(int val, bool triggerEvents = true) {
        int lifeResult = LifeValue + val;
        int willLoss = 0;
        while (lifeResult < 1) {
            lifeResult += 20;
            willLoss++;
        }
        base.UpdateLifeValue(lifeResult - LifeValue, triggerEvents);
        UpdateWillValue(-willLoss);
    }

    public void UpdateMaxWill(int val) {
        MaxWill += val;
    }

    public void UpdateWillValue(int val) {
        WillValue += val;
        if (WillValue > MaxWill) {
            // Will value cannot exceed max will
            WillValue = MaxWill;
        }
    }

    public new void ReceiveAttack(Attacker attacker) {
        // Invoke the OnAttack event before dealing damage
        // Allows for buffs on attack triggers before damage is dealt
        OnAttack?.Invoke(attacker, this);
        UpdateLifeValue(-attacker.AttackValue, false);
        // Invoke damage from attack event
        OnDamageAttack?.Invoke(attacker, this, attacker.AttackValue, LifeValue);
    }

    public override void PerformAttack() {
        display.AnimateAttack();
    }

    #endregion
}
