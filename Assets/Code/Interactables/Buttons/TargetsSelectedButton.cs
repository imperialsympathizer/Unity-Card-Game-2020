using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetsSelectedButton : MonoBehaviour, IPointerClickHandler {
    // Event to fire when button is pressed
    public static event Action OnTargetsSelectedClicked;

    public static void ClearSubscriptions() {
        OnTargetsSelectedClicked = null;
    }

    public void OnPointerClick(PointerEventData eventData) {
        // Fires the event when the end turn button is clicked
        OnTargetsSelectedClicked.Invoke();
    }
}
