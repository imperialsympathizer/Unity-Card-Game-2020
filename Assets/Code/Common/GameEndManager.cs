using TMPro;
using UnityEngine;

public class GameEndManager {
    public static GameEndManager SharedInstance;

    private GameObject prefab;
    private GameObject endDisplay;

    private bool battleOver;

    public void Initialize() {
        SharedInstance = this;
        battleOver = false;
        prefab = VisualController.SharedInstance.GetPrefab("BattleEnd");
    }

    public void ShowGameEnd(bool win) {
        if (!battleOver) {
            battleOver = true;

            endDisplay = Object.Instantiate(prefab);
            endDisplay.SetActive(true);

            // Move the visual to the front
            endDisplay.transform.SetAsLastSibling();
            endDisplay.transform.position = Vector3.zero;
            GameObject canvas = endDisplay.transform.GetChild(0).gameObject;
            canvas.GetComponent<Canvas>().worldCamera = VisualController.SharedInstance.mainCamera;
            TextMeshPro text = canvas.transform.GetChild(canvas.transform.childCount - 1).GetComponent<TextMeshPro>();
            endDisplay.transform.localScale = Vector3.one;

            // Make the canvas active (needs to be done prior to animations)

            // Set initial alpha of sprites to 0
            LeanTween.alpha(canvas, 0, 0);

            // TextMeshPro is a bit tricky to fade using LT, have to use value to do so
            if (win) {
                text.text = "VICTORY";
                LeanTween.value(text.gameObject, a => text.color = a, new Color(0.2f, 0.6f, 1, 0), new Color(0.2f, 0.6f, 1, 1), 3);
            }
            else {
                text.text = "GAME OVER";
                LeanTween.value(text.gameObject, a => text.color = a, new Color(0.8f, 0, 0, 0), new Color(0.8f, 0, 0, 1), 3);
            }
            // Fade in the canvas (including black background)
            LeanTween.alpha(canvas, 1, 3);
            LeanTween.alphaCanvas(canvas.GetComponent<CanvasGroup>(), 1, 3);

            // These loops swap the particle falling sprites to the correct clips so that the animations aren't all in sync
            Animator sprite;
            for (int i = 1; i < 5; i++) {
                sprite = canvas.transform.GetChild(i).GetComponent<Animator>();
                sprite.SetInteger("particleAnimation", 6);
            }

            for (int i = 5; i < 9; i++) {
                sprite = canvas.transform.GetChild(i).GetComponent<Animator>();
                sprite.SetInteger("particleAnimation", 1);
            }

            for (int i = 9; i < 13; i++) {
                sprite = canvas.transform.GetChild(i).GetComponent<Animator>();
                sprite.SetInteger("particleAnimation", 4);
            }

            for (int i = 13; i < 17; i++) {
                sprite = canvas.transform.GetChild(i).GetComponent<Animator>();
                sprite.SetInteger("particleAnimation", 2);
            }

            for (int i = 17; i < 21; i++) {
                sprite = canvas.transform.GetChild(i).GetComponent<Animator>();
                sprite.SetInteger("particleAnimation", 5);
            }

            for (int i = 21; i < 25; i++) {
                sprite = canvas.transform.GetChild(i).GetComponent<Animator>();
                sprite.SetInteger("particleAnimation", 3);
            }
        }
    }
}
