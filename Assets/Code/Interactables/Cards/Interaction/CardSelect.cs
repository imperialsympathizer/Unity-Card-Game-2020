using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelect : MonoBehaviour, IPointerClickHandler {
    // Event to fire when button is pressed
    public static event Action<int> OnCardSelect;

    public int cardId;

    public void OnPointerClick(PointerEventData eventData) {
        // Fires the event when the end turn button is clicked
        // Debug.Log("button pressed");
        OnCardSelect?.Invoke(cardId);
    }
}
