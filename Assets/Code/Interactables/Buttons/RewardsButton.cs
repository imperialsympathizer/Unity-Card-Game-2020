using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class RewardsButton : MonoBehaviour, IPointerClickHandler {
    // Event to fire when button is pressed
    public static event Action OnRewardsButtonClicked;

    public static void ClearSubscriptions() {
        OnRewardsButtonClicked = null;
    }

    // TODO: implement a check for the E button being pressed to end the turn as well;

    public void OnPointerClick(PointerEventData eventData) {
        // Fires the event when the end turn button is clicked
        // Debug.Log("button pressed");
        OnRewardsButtonClicked.Invoke();
    }
}
