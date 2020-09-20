using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour {
    public bool Dragging { get; private set; }
    public bool OverOriginalParent { get; private set; }

    private GameObject placeholder;
    private CanvasGroup placeholderAlphaControl;
    private Transform originalParent;
    private int originalSiblingIndex;

    
    // The sibling index of the object as it is being dragged over the parent
    private int currentIndex;

    public void Init(GameObject placeholder) {
        this.placeholder = placeholder;
        this.placeholder.SetActive(false);
        // The placeholder object needs a Canvas Group component to set its alpha, so ensure it has one
        if (!this.placeholder.TryGetComponent<CanvasGroup>(out placeholderAlphaControl)) {
            placeholderAlphaControl = this.placeholder.AddComponent<CanvasGroup>();
        }
        VisualController.SharedInstance.RemoveFromVisual(this.placeholder.transform);
    }

    // This method is to be used before animations or movement of the card object
    public void SetOriginData() {
        originalParent = this.gameObject.transform.parent;
        originalSiblingIndex = this.gameObject.transform.GetSiblingIndex();
        currentIndex = originalSiblingIndex;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        Dragging = true;
        OverOriginalParent = true;
        SetOriginData();

        // Move the object being dragged to the game canvas layer so it is visible above everything else
        VisualController.SharedInstance.ParentToInteractableCanvas(this.gameObject.transform);

        // Parent the placeholder (invisible) to the original parent so everything looks the same
        // Creates a "shadow" preview of where the item is going to be dropped
        placeholderAlphaControl.alpha = 0.5f;
        placeholder.transform.SetParent(originalParent);
        placeholder.transform.SetSiblingIndex(originalSiblingIndex);
        placeholder.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData) {
        // Set the object's position to the pointer
        this.gameObject.transform.position.Set(eventData.position.x, eventData.position.y, 0);

        // Check if the position of the dragged object is over the original parent or not
        Rect rect = originalParent.GetComponent<RectTransform>().rect;

        float xLoc = eventData.position.x;
        float yLoc = eventData.position.y;

        // Check the x position
        if (xLoc < rect.position.x + rect.width / 2 && xLoc > rect.position.x - rect.height / 2) {
            // Check the y position
            if (yLoc < rect.position.y + rect.width / 2 && yLoc > rect.position.y - rect.height / 2) {
                OverOriginalParent = true;
            }
        }

        // If the object is over the original parent, get its sibling index as a function of its location and put the placeholder there
        // This moves the "preview" around of where the object will go once dropped
        // Otherwise, disable the placeholder's visual and remove it from the originalParent
        placeholder.SetActive(false);
        if (OverOriginalParent) {
            currentIndex = GetSiblingIndexFromPosition(xLoc);
            placeholder.transform.SetParent(originalParent);
            placeholder.transform.SetSiblingIndex(currentIndex);
            placeholder.SetActive(true);
        }
        else {
            VisualController.SharedInstance.RemoveFromVisual(placeholder.transform);
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        Dragging = false;
        // Deactivate the placeholder and remove from visual
        placeholder.SetActive(false);
        VisualController.SharedInstance.RemoveFromVisual(placeholder.transform);

        // The object will always snap back to the original parent at the CURRENT index
        // This allows for sorting of draggable objects on a specific parent
        // Note: overOriginalParent will still be false if it was dropped outside of the parent zone
        // This can be useful for when objects are meant to be dropped in other areas (Example: deactivate a card and play it)
        this.gameObject.transform.SetParent(originalParent);
        this.gameObject.transform.SetSiblingIndex(currentIndex);
    }

    // Currently only supports indexing on the x axis
    public int GetSiblingIndexFromPosition(float currentX) {
        int childCount = originalParent.transform.childCount;
        // Default index is at the very right
        int newIndex = childCount;

        for (int i = 0; i < childCount; i++) {
            // Iterate from left to right over the original parent, checking the dragged object's position against every child's
            if (currentX < originalParent.transform.GetChild(i).transform.position.x) {
                newIndex = i;
                // If the dragged object is left of a child, but the placeholder is already 1 position left of the child, do not change placeholder position
                if (i > 0 && originalParent.transform.GetChild(i - 1).gameObject == placeholder) {
                    newIndex--;
                }
                break;
            }
        }

        return newIndex;
    }
}
