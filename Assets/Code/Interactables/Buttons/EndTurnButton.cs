using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndTurnButton : MonoBehaviour, IPointerClickHandler {
    // Event to fire when button is pressed
    public static event Action OnEndTurnClicked;

    // TODO: implement a check for the E button being pressed to end the turn as well;

    public static void ClearSubscriptions() {
        OnEndTurnClicked = null;
    }

    public void OnPointerClick(PointerEventData eventData) {
        // Fires the event when the end turn button is clicked
        // Debug.Log("button pressed");
        OnEndTurnClicked?.Invoke();
    }
}
