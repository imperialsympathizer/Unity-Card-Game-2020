using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    // Event to fire when button is pressed
    public static event Action OnMenuButtonClicked;

    public GameObject outline;

    // TODO: implement a check for the E button being pressed to end the turn as well;

    private void Awake() {
        outline.SetActive(false);
    }

    public static void ClearSubscriptions() {
        OnMenuButtonClicked = null;
    }

    public void OnPointerClick(PointerEventData eventData) {
        // Fires the event when the end turn button is clicked
        // Debug.Log("button pressed");
        OnMenuButtonClicked?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        outline.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        outline.SetActive(false);
    }
}
