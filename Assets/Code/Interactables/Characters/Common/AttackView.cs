using TMPro;
using UnityEngine;

public class AttackView : MonoBehaviour {
    private Fighter fighter;
    public TextMeshProUGUI attackValue;
    public TextMeshProUGUI attackTimes;
    public TextMeshProUGUI xText;

    public void InitializeView(Fighter fighter) {
        this.fighter = fighter;
        fighter.OnAttackChange += SetAttackValues;
        SetAttackValues(fighter.AttackValue, fighter.AttackTimes);
    }

    public void DisableView() {
        fighter.OnAttackChange -= SetAttackValues;
        fighter = null;
    }

    private void SetAttackValues(int attackValue, int attackTimes) {
        this.attackValue.text = attackValue.ToString();
        this.attackTimes.text = attackTimes.ToString();
        // If the val is 1, disable the x and AttackTimes displays
        // If val is 0, disable all attack displays
        if (attackTimes == 1) {
            xText.text = attackValue.ToString();
            xText.fontSize = 25;
            this.attackValue.gameObject.SetActive(false);
            this.attackTimes.gameObject.SetActive(false);
            xText.gameObject.SetActive(true);
            // Use the xText display to display attack since it is the box centered over the character
            // it will be used as the attackValue display unless a character has attackTimes > 1
        }
        else if (attackTimes == 0) {
            this.attackValue.gameObject.SetActive(false);
            this.attackTimes.gameObject.SetActive(false);
            xText.gameObject.SetActive(false);
        }
        else {
            // Make sure to reset the xText
            xText.text = "x";
            xText.fontSize = 18;
            this.attackValue.gameObject.SetActive(true);
            this.attackTimes.gameObject.SetActive(true);
            xText.gameObject.SetActive(true);
        }
    }
}
