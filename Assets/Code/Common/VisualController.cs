using System;
using System.Collections.Generic;
using UnityEngine;

public class VisualController : BaseController {
    public static VisualController Instance;

    public Camera mainCamera;

    // The 4 large umbrella canvases that house the other visuals
    private GameObject displayCanvas;
    private GameObject backgroundCanvas;
    private GameObject characterCanvas;
    private GameObject interactableCanvas;

    // The "smaller" canvases that require frequent parenting
    private GameObject icons;
    private GameObject players;

    private GameObject slots;
    private GameObject summons;
    private GameObject enemies;
    private GameObject hand;
    private GameObject artifacts;

    // Nothing should ever be parented to this, it is just a rect for checking if cards have been played
    private GameObject playZone;

    // The GameController, where objects can be parented to remove from any visual canvases
    private GameObject gameController;

    // Prefabs must be loaded by the ResourceController and passed as arguments to the Initialize method
    private Dictionary<string, GameObject> prefabs;
    private Dictionary<string, Sprite> images;

    protected override bool Initialize() {
        Instance = this;

        // Main camera is sometimes useful for positional calculation,
        // But calling Camera.main every time is is needed is VERY expensive
        // Better to have it cached for when it is necessary
        mainCamera = Camera.main;

        // Initialize prefabs
        prefabs = new Dictionary<string, GameObject>(ResourceController.prefabDictionary);
        images = new Dictionary<string, Sprite>(ResourceController.spriteDictionary);

        // Game controller
        gameController = GameObject.Find("GameController");

        // Display canvas
        displayCanvas = GameObject.Find("Display");

        // Background canvas
        backgroundCanvas = displayCanvas.transform.GetChild(0).gameObject;
        icons = backgroundCanvas.transform.GetChild(4).gameObject;

        // Artifact canvas
        artifacts = displayCanvas.transform.GetChild(1).gameObject;

        // Character canvas
        characterCanvas = displayCanvas.transform.GetChild(2).gameObject;
        players = characterCanvas.transform.GetChild(0).gameObject;
        slots = characterCanvas.transform.GetChild(1).gameObject;
        summons = characterCanvas.transform.GetChild(2).gameObject;
        enemies = characterCanvas.transform.GetChild(3).gameObject;

        // Interactable canvas
        interactableCanvas = displayCanvas.transform.GetChild(3).gameObject;
        playZone = interactableCanvas.transform.GetChild(0).gameObject;
        hand = interactableCanvas.transform.GetChild(1).gameObject;

        return true;
    }

    public GameObject GetPrefab(string prefabKey) {
        if (prefabs.TryGetValue(prefabKey, out GameObject prefab)) {
            return prefab;
        }

        return null;
    }

    public Sprite GetImage(string imageKey) {
        if (images.TryGetValue(imageKey, out Sprite image)) {
            return image;
        }

        return null;
    }

    public GameObject GetPlayerCanvas() {
        return players;
    }

    public GameObject GetSummonCanvas() {
        return summons;
    }

    public GameObject GetEnemyCanvas() {
        return enemies;
    }

    #region Parenting Functions

    public void RemoveFromVisual(Transform transform) {
        transform.SetParent(gameController.transform, true);
    }

    public void ParentToDisplayCanvas(Transform transform) {
        transform.SetParent(displayCanvas.transform, true);
    }

    public void ParentToCharacterCanvas(Transform transform) {
        transform.SetParent(characterCanvas.transform, true);
    }

    public void ParentToPlayerCanvas(Transform transform) {
        transform.SetParent(players.transform, true);
    }

    public void ParentToSlotCanvas(Transform transform) {
        transform.SetParent(slots.transform, true);
    }

    public void ParentToEnemyCanvas(Transform transform) {
        transform.SetParent(enemies.transform, true);
    }

    public void ParentToSummonCanvas(Transform transform) {
        transform.SetParent(summons.transform, true);
    }

    public void ParentToArtifactCanvas(Transform transform) {
        transform.SetParent(artifacts.transform, true);
    }

    // This is the bottom layer object (therefore is at the front of the screen)
    // Any objects that need to be shown above everything else should be parented to this
    public void ParentToInteractableCanvas(Transform transform) {
        transform.SetParent(interactableCanvas.transform);
    }

    public void ParentToHand(Transform transform, int siblingIndex = -1) {
        transform.SetParent(hand.transform);
        if (siblingIndex >= 0 && siblingIndex < hand.transform.childCount) {
            transform.SetSiblingIndex(siblingIndex);
        }
    }

    #endregion

    public Rect GetPlayzoneRect() {
        return playZone.GetComponent<RectTransform>().rect;
    }

    public Vector2 GetDisplaySize() {
        return displayCanvas.GetComponent<RectTransform>().sizeDelta;
    }

    public GameObject GetHand() {
        return hand;
    }

    public GameObject GetDeckCount() {
        return icons.transform.GetChild(0).GetChild(1).gameObject;
    }

    public GameObject GetDiscardCount() {
        return icons.transform.GetChild(1).GetChild(1).gameObject;
    }

    public GameObject GetWillIcon() {
        return icons.transform.GetChild(2).gameObject;
    }

    public GameObject GetLifeIcon() {
        return icons.transform.GetChild(3).gameObject;
    }

    public GameObject GetVigorIcon() {
        return icons.transform.GetChild(4).gameObject;
    }
}
