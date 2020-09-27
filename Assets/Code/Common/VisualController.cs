using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualController {
    public static VisualController SharedInstance;

    public Camera mainCamera;

    // The 4 large umbrella canvases that house the other visuals
    private GameObject displayCanvas;
    private GameObject backgroundCanvas;
    private GameObject characterCanvas;
    private GameObject interactableCanvas;

    // The "smaller" canvases that require frequent parenting
    private GameObject players;
    private GameObject slots;
    private GameObject summons;
    private GameObject enemies;
    private GameObject hand;
    // Nothing should ever be parented to this, it is just a rect for checking if cards have been played
    private GameObject playZone;

    // Game over canvas, should always be disabled unless receiving a game over event
    private GameObject gameOver;

    // The GameController, where objects can be parented to remove from any visual canvases
    private GameObject gameController;

    // Prefabs must be loaded by the ResourceController and passed as arguments to the Initialize method
    private Dictionary<string, GameObject> prefabs;

    //public GameObject cardPrefab;
    //public GameObject necromancerPrefab;
    //public GameObject slotPrefab;
    //public GameObject zombiePrefab;
    //public GameObject knightPrefab;


    public void Initialize(Dictionary<string, GameObject> prefabs) {
        SharedInstance = this;

        // Main camera is sometimes useful for positional calculation,
        // But calling Camera.main every time is is needed is VERY expensive
        // Better to have it cached for when it is necessary
        mainCamera = Camera.main;

        // Initialize prefabs
        this.prefabs = prefabs;

        // Game controller
        gameController = GameObject.Find("GameController");

        // Display canvas
        displayCanvas = GameObject.Find("Display");

        // Background canvas
        backgroundCanvas = displayCanvas.transform.GetChild(0).gameObject;

        // Interactable canvas
        interactableCanvas = displayCanvas.transform.GetChild(2).gameObject;
        playZone = interactableCanvas.transform.GetChild(0).gameObject;
        hand = interactableCanvas.transform.GetChild(1).gameObject;

        // Character canvas
        characterCanvas = displayCanvas.transform.GetChild(1).gameObject;
        players = characterCanvas.transform.GetChild(0).gameObject;
        slots = characterCanvas.transform.GetChild(1).gameObject;
        summons = characterCanvas.transform.GetChild(2).gameObject;
        enemies = characterCanvas.transform.GetChild(3).gameObject;

        // Game Over canvas
        gameOver = displayCanvas.transform.GetChild(3).gameObject;
    }

    public GameObject GetPrefab(string prefabKey) {
        return prefabs[prefabKey];
    }

    #region Parenting Functions

    public void RemoveFromVisual(Transform transform) {
        transform.SetParent(gameController.transform);
    }

    public void ParentToDisplayCanvas(Transform transform) {
        transform.SetParent(displayCanvas.transform);
    }

    public void ParentToCharacterCanvas(Transform transform) {
        transform.SetParent(characterCanvas.transform);
    }

    public void ParentToPlayerCanvas(Transform transform) {
        transform.SetParent(players.transform);
    }

    public void ParentToSlotCanvas(Transform transform) {
        transform.SetParent(slots.transform);
    }

    public void ParentToEnemyCanvas(Transform transform) {
        transform.SetParent(enemies.transform);
    }

    public void ParentToSummonCanvas(Transform transform) {
        transform.SetParent(summons.transform);
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

    public GameObject GetGameOver() {
        return gameOver;
    }

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
        return backgroundCanvas.transform.GetChild(3).GetChild(1).GetChild(2).gameObject;
    }

    public GameObject GetDiscardCount() {
        return backgroundCanvas.transform.GetChild(3).GetChild(2).GetChild(2).gameObject;
    }

    public GameObject GetLifeValue() {
        return backgroundCanvas.transform.GetChild(3).GetChild(3).GetChild(0).gameObject;
    }

    public GameObject GetWillValue() {
        return backgroundCanvas.transform.GetChild(3).GetChild(4).GetChild(0).gameObject;
    }
}
