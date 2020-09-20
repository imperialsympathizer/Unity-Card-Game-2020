using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMouseInteraction : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {
    public GameObject placeholder;

    public bool inHand = true;

    public int cardId;
    // private float enterTime = 0;
    // private float delay = 0.5f;
    private float heightPadding = 0;
    // index of the card's position in hand when it is picked up
    public int handIndex;
    public Transform handTransform;
    // index of the card's position while being dragged
    private int currentIndex;

    private bool zooming;
    private bool dragging;

    private bool hovering = false;

    public static event Action<int> OnCardPlayed;

    private Vector3 mousePos;

    public void OnPointerEnter(PointerEventData eventData) {
        hovering = true;
        inHand = true;

        // This is to prevent the zoomed card from being cut off at the bottom of the screen
        heightPadding = 1;
    }

    public void OnPointerExit(PointerEventData eventData) {
        // enterTime = 0;
        hovering = false;
        if (zooming) {
            LeanTween.cancel(this.gameObject);
            this.transform.localScale = new Vector3(1, 1, 1);
            VisualController.SharedInstance.ParentToHand(this.transform, handIndex);
            placeholder.SetActive(false);
            VisualController.SharedInstance.RemoveFromVisual(placeholder.transform);
            zooming = false;
        }
    }

    void Update() {
        if (!zooming && !dragging && hovering) {
            zooming = true;
            // Update the current handIndex
            handIndex = this.transform.GetSiblingIndex();
            // Move the object being dragged to the game canvas layer so it is visible above everything else
            VisualController.SharedInstance.ParentToInteractableCanvas(this.gameObject.transform);

            // Parent the placeholder (invisible) where the zoom object originally was so everything looks the same
            placeholder.SetActive(false);
            placeholder.GetComponent<CanvasGroup>().alpha = 0.0f;
            VisualController.SharedInstance.ParentToHand(placeholder.transform, handIndex);
            placeholder.transform.localScale = new Vector3(1, 1, 1);
            placeholder.SetActive(true);

            LeanTween.scale(gameObject, new Vector3(2, 2, 2), 0.2f);
            LeanTween.move(gameObject, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + heightPadding), 0.2f);
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        dragging = true;
        hovering = false;
        // end zoom if zooming
        // enterTime = 0;
        if (zooming) {
            zooming = false;
            LeanTween.cancel(this.gameObject);
            VisualController.SharedInstance.ParentToHand(this.transform, handIndex);
        }

        // Make the card being played slightly larger, but not as much as a zoomed card
        LeanTween.scale(gameObject, new Vector3(1.3f, 1.3f, 1.3f), 0.15f);

        // If the card is ever moved to another zone from the hand, dropLocation will change
        inHand = true;

        // This method is to be used before animations or movement of the card object
        // Move the object being dragged to the interactable canvas layer
        handIndex = this.transform.GetSiblingIndex();
        VisualController.SharedInstance.ParentToInteractableCanvas(this.transform);

        // Move the placeholder object to the hand for padding, alpha > 0 makes a "shadow effect"
        placeholder.SetActive(false);
        placeholder.GetComponent<CanvasGroup>().alpha = 0.5f;
        VisualController.SharedInstance.ParentToHand(placeholder.transform, handIndex);
        placeholder.transform.localScale = new Vector3(1, 1, 1);
        placeholder.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData) {
        // Move the object location to the pointer location relative to where it was picked up
        var pos = VisualController.SharedInstance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = new Vector3(pos.x, pos.y, 0);

        // Check if the position of the dragged object is over the original parent or not
        Rect rect = handTransform.GetComponent<RectTransform>().rect;
        float yLoc = eventData.position.y;

        inHand = false;
        // Check the y position
        if (yLoc < rect.height) {
            inHand = true;
        }

        // If the card is being dragged in the hand, create the "shadow" placeholder card and move it
        if (inHand) {
            currentIndex = GetSiblingIndexFromPosition(this.transform.position.x);
            placeholder.SetActive(false);
            VisualController.SharedInstance.ParentToHand(placeholder.transform, currentIndex);
            placeholder.transform.localScale = new Vector3(1, 1, 1);
            placeholder.SetActive(true);
        }
        else {
            placeholder.SetActive(false);
            VisualController.SharedInstance.RemoveFromVisual(placeholder.transform);
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        dragging = false;
        placeholder.SetActive(false);

        // When the card is dropped, if it's over the "play zone", play the card
        // Otherwise, snap it back to the hand at its most recent index
        Rect rect = handTransform.GetComponent<RectTransform>().rect;
        float yLoc = eventData.position.y;

        inHand = false;

        if (yLoc < rect.height) {
            inHand = true;
        }

        // Transform the card back to its hand size (it might be reused later)
        gameObject.SetActive(false);
        gameObject.transform.localScale = new Vector3(1, 1, 1);

        if (!inHand) {
            // Play the card, passing card id so that relevant entities know which card was played
            Debug.Log("playing card.");
            OnCardPlayed.Invoke(cardId);
        }
        else {
            VisualController.SharedInstance.ParentToHand(this.transform, currentIndex);
            VisualController.SharedInstance.RemoveFromVisual(placeholder.transform);
            gameObject.SetActive(true);
        }
    }

    // Currently only supports indexing on the x axis
    public int GetSiblingIndexFromPosition(float currentX) {
        int childCount = handTransform.childCount;
        // Default index is at the very right
        int newIndex = childCount;

        for (int i = 0; i < childCount; i++) {
            // Iterate from left to right over the original parent, checking the dragged object's position against every child's
            if (currentX < handTransform.GetChild(i).transform.position.x) {
                newIndex = i;
                break;
            }
        }

        return newIndex;
    }
}
