using System;
using UnityEngine;

public class Player : Fighter {
    // Class that houses the data for players
    // Contains references to PlayerView but does not directly control it in most instances
    // Accessed and instantiated through the PlayerController
    public PlayerView display;

    public int MaxWill { get; private set; }
    public int WillValue { get; private set; }
    public int VigorValue { get; private set; }

    public bool HasSlots { get; private set; }
    public int MaxSlots { get; private set; }
    public int SlotsValue { get; private set; }

    // Visual component of the player, stored within its own View class
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
        int slotsValue = 0) : base(name, Fighter.FighterType.PLAYER, baseAttack, baseAttackTimes, true, baseMaxLife, baseLife) {
        this.prefab = prefab;
        slotPrefab = VisualController.Instance.GetPrefab("SlotPrefab");
        MaxWill = baseMaxWill;
        WillValue = baseWillValue;
        HasSlots = hasSlots;
        MaxSlots = maxSlots;
        SlotsValue = slotsValue;
    }

    #region Visual Methods

    public override void CreateVisual() {
        // Spawn an object to view the player on screen
        // Not using the ObjectPooler as there is only one player character
        GameObject playerVisual = GameObject.Instantiate(prefab, new Vector3(0, 0, -10), Quaternion.identity);
        display = new PlayerView(playerVisual, id, slotPrefab, SlotsValue);
        UpdateVisual();
    }

    public override RectTransform GetVisualRect() {
        return display.getVisualRect();
    }

    public override void SetVisualOutline(Color color) {
        display.SetVisualOutline(color);
    }

    public override void ClearVisual() {
        if (display != null) {
            display.Despawn();
        }
    }

    public override void UpdateVisual() {
        display.SetActive(false);
        display.SetAttack(AttackValue);
        display.SetAttackTimes(AttackValue, AttackTimes);
        display.SetLife(true, LifeValue, MaxLife);
        display.SetWill(WillValue, MaxWill);
        display.SetVigor(VigorValue, MaxLife);
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

    public void UpdateVigorValue(int val) {
        VigorValue += val;
        UpdateVisual();
    }

    public new bool UpdateLifeValue(int val, bool triggers = false) {
        // Lose 1 will for each max life required to get LifeValue back to positive
        int lifeResult = LifeValue + val;
        int willResult = WillValue;
        while (lifeResult < 1) {
            willResult--;
            lifeResult += MaxLife;
        }

        // On the player's turn, if they heal, they regain that energy to spend
        if (val > 0 && TurnSystem.Instance.IsPlayerTurn()) {
            int vigorResult = VigorValue + val;
            vigorResult = vigorResult > MaxLife ? MaxLife : vigorResult;
            UpdateVigorValue(vigorResult - VigorValue);
        }

        UpdateWillValue(willResult - WillValue);
        return base.UpdateLifeValue(lifeResult - LifeValue, triggers);
    }

    public void UpdateMaxWill(int val) {
        MaxWill += val;
        if (WillValue > MaxLife) {
            UpdateWillValue(0);
        }
        else {
            UpdateVisual();
        }
    }

    public void UpdateWillValue(int val) {
        WillValue += val;
        if (WillValue > MaxWill) {
            // Will value cannot exceed max will
            WillValue = MaxWill;
        }
        UpdateVisual();
    }

    public new void ReceiveAttack(Fighter attacker) {
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

    public override void EnableVisual(bool enable) {
        throw new NotImplementedException();
    }

    #endregion
}
