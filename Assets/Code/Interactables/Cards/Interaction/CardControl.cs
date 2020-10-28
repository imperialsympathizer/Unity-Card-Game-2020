using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {
    public CurvedLayout hand;

    public int cardId;

    // index of the card's position in hand when it is picked up
    private int handIndex;

    private float interpolation;
    private float increment = 0.03f;

    private bool inHand = true;
    private bool zooming = false;
    private bool dragging = false;

    public static event Action<int> OnCardPlayed;

    public static void ClearSubscriptions() {
        OnCardPlayed = null;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!dragging) {
            zooming = true;

            // Get the current hand index
            handIndex = this.transform.GetSiblingIndex();

            // Get the current position to be updated
            // Use this method instead of transform.position because the card may be in the middle of an animation so doesn't reflect where it should be
            Vector3 newPosition = hand.WhereShouldCardBe(handIndex);

            hand.UpdateCardPositions(true, handIndex);

            // Move the object being zoomed to the game canvas layer so it is visible above everything else
            VisualController.Instance.ParentToInteractableCanvas(this.transform);

            // Scale the card up so that it is easy to read
            float scale = 1.5f;
            LeanTween.scale(gameObject, new Vector3(scale, scale, scale), 0.2f);

            // Move the card so that its bottom edge is at the bottom edge of the screen
            // New y position is half the card's NEW height minus half the screen's height
            float canvasHeight = VisualController.Instance.GetDisplaySize().y;
            float cardHeight = this.transform.GetComponent<RectTransform>().sizeDelta.y * scale;

            newPosition.y = cardHeight / 2 - canvasHeight / 2;

            LeanTween.moveLocal(this.gameObject, newPosition, 0.2f);

            // Reset any rotation
            LeanTween.rotate(this.gameObject, new Vector3(0, 0, 0), 0.2f);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (zooming) {
            zooming = false;

            // Reparent to the hand at the handIndex
            this.transform.SetParent(hand.transform);
            this.transform.SetSiblingIndex(handIndex);
            hand.UpdateCardPositions();
        }
    }

    private void Update() {
        if (dragging) {
            // Use linear interpolation to smoothly move the card's origin toward the pointer
            // Then, keep it snapped to the pointer

            Vector3 mousePos = VisualController.Instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (interpolation < 1) {
                Vector3 currentPos = this.transform.position;
                Vector3 newPos = Vector3.Lerp(currentPos, mousePos, interpolation);
                this.transform.position = new Vector3(newPos.x, newPos.y, 0);
                interpolation += increment;
            }
            else {
                this.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
            }

            // Check if the position of the dragged object is over the hand or not
            float handYThreshold = hand.GetComponent<RectTransform>().sizeDelta.y / 2;
            float yLoc = hand.transform.InverseTransformPoint(mousePos).y;

            inHand = false;
            // Check the y position
            if (yLoc < handYThreshold) {
                inHand = true;
            }

            // If the card is over the hand, adjust the hand accordingly
            if (inHand) {
                handIndex = GetSiblingIndexFromPosition(this.transform.localPosition.x);
                hand.UpdatePositionsFromDrag(handIndex);
            }
        }
    }

    private int GetSiblingIndexFromPosition(float currentX) {
        int childCount = hand.transform.childCount;
        // Default index is at the very right
        int newIndex = childCount;

        for (int i = 0; i < childCount; i++) {
            Transform child = hand.transform.GetChild(i).transform;
            // Iterate from left to right over the original parent, checking the dragged object's position against every child's
            if (currentX < child.localPosition.x) {
                newIndex = i;
                break;
            }
        }

        return newIndex;
    }

    public void OnPointerDown(PointerEventData eventData) {
        // Cannot drag a card when selecting cards for effects
        if (!TargetSelector.Instance.Selecting) {
            if (!dragging) {
                zooming = false;
                dragging = true;
                interpolation = 0;
                LeanTween.cancel(this.gameObject);
                // scale the card being dragged back down
                LeanTween.scale(this.gameObject, new Vector3(1f, 1f, 1f), 0.2f);
            }

            // Get the current hand index
            handIndex = this.transform.GetSiblingIndex();

            // Move the object being dragged to the game canvas layer so it is visible above everything else
            VisualController.Instance.ParentToInteractableCanvas(this.transform);
            hand.UpdatePositionsFromDrag(handIndex);
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        dragging = false;

        if (inHand) {
            // Move the object being dragged back to the hand at its last recorded index
            VisualController.Instance.ParentToHand(this.transform, handIndex);
            hand.UpdateCardPositions();
        }
        else {
            // Reset parameters in case card play is cancelled
            inHand = true;
            dragging = false;
            zooming = false;
            interpolation = 0;
            OnCardPlayed?.Invoke(cardId);
            hand.UpdateCardPositions();
        }
    }
}
