using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Zoomable : MonoBehaviour {
    public bool Zooming { get; private set; }
    public bool MouseOver { get; private set; }

    private float timer = 0;
    private float delayTime = 0.5f;

    // TODO: instantiate better
    private float zoomScale = 1;

    private GameObject placeholder;
    private CanvasGroup placeholderAlphaControl;
    private Transform originalParent;
    private int originalSiblingIndex;
    private Vector3 originalScale;
    
    // Returns a reference to the specific object that is zooming so subscribers to the event
    // Know whether to care about it or not.
    public static Action<GameObject> OnZoom;
    public static Action<GameObject> OnZoomCancel;

    public void Init(GameObject placeholder, float zoomScale) {
        this.zoomScale = zoomScale;
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
        originalScale = this.gameObject.transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        MouseOver = true;
        timer = Time.time + delayTime;
    }

    public void OnPointerExit(PointerEventData eventData) {
        CancelZoom();
    }

    public void Zoom() {
        // Can only zoom in on cards during player turn
        if (!Zooming && TurnSystem.SharedInstance.IsPlayerTurn()) {
            Zooming = true;

            // Set the origin data so the object knows where to return to when the zoom finishes
            SetOriginData();

            // Move the object being dragged to the game canvas layer so it is visible above everything else
            VisualController.SharedInstance.ParentToInteractableCanvas(this.gameObject.transform);

            // Parent the placeholder (invisible) where the zoom object originally was so everything looks the same
            placeholderAlphaControl.alpha = 0.0f;
            placeholder.transform.SetParent(originalParent);
            placeholder.transform.SetSiblingIndex(originalSiblingIndex);
            placeholder.SetActive(true);

            LeanTween.scale(this.gameObject, new Vector3(zoomScale, zoomScale, zoomScale), 0.2f);
            LeanTween.move(this.gameObject, new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 100), 0.2f);
        }
    }

    public void CancelZoom() {
        Zooming = false;
        MouseOver = false;
        timer = 0;

        // Remove the placeholder from the visual
        placeholder.SetActive(false);
        VisualController.SharedInstance.RemoveFromVisual(this.placeholder.transform);

        // Disable zoomed object temporarily to make changes that don't force dirty updates on the canvas
        this.gameObject.SetActive(false);
        // Cancel any tween events on the object
        LeanTween.cancel(this.gameObject);
        // Revert to object's original scale
        this.gameObject.transform.localScale = originalScale;
        // Parent back to original parent and index
        this.gameObject.transform.SetParent(originalParent);
        this.gameObject.transform.SetSiblingIndex(originalSiblingIndex);
        // Reactivate the zoom object
        this.gameObject.SetActive(true);
    }

    void Update() {
        if (Time.time > timer && timer != 0) {
            Zoom();
        }
    }
}
