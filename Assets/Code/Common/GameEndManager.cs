using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

class GameEndManager {
    public static GameEndManager SharedInstance;

    private GameObject canvas;

    public void Initialize() {
        SharedInstance = this;

        canvas = VisualController.SharedInstance.GetGameOver();
        canvas.SetActive(false);
    }

    public void ShowGameEnd(bool win) {
        // Set initial alpha of sprites to 0
        LeanTween.alpha(canvas, 0, 0);

        // TextMeshPro is a bit tricky to fade using LT, have to use value to do so
        TextMeshPro text;
        if (win) {
            text = canvas.transform.GetChild(canvas.transform.childCount - 1).GetComponent<TextMeshPro>();
            LeanTween.value(text.gameObject, a => text.color = a, new Color(0.2f, 0.6f, 1, 0), new Color(0.2f, 0.6f, 1, 1), 3);
        }
        else {
            text = canvas.transform.GetChild(canvas.transform.childCount - 2).GetComponent<TextMeshPro>();
            LeanTween.value(text.gameObject, a => text.color = a, new Color(0.8f, 0, 0, 0), new Color(0.8f, 0, 0, 1), 3);
        }
        // Fade in the canvas (including black background)
        LeanTween.alpha(canvas, 1, 3);
        LeanTween.alphaCanvas(canvas.GetComponent<CanvasGroup>(), 1, 3);

        // Make the canvas active (needs to be done prior to animation swapping)
        canvas.SetActive(true);
        
        // These loops swap the particle falling sprites to the correct clips so that the animations aren't all in sync
        for (int i = 1; i < 5; i++) {
            Animator sprite = canvas.transform.GetChild(i).GetComponent<Animator>();
            sprite.SetInteger("particleAnimation", 6);
        }

        for (int i = 5; i < 9; i++) {
            Animator sprite = canvas.transform.GetChild(i).GetComponent<Animator>();
            sprite.SetInteger("particleAnimation", 1);
        }

        for (int i = 9; i < 13; i++) {
            Animator sprite = canvas.transform.GetChild(i).GetComponent<Animator>();
            sprite.SetInteger("particleAnimation", 4);
        }

        for (int i = 13; i < 17; i++) {
            Animator sprite = canvas.transform.GetChild(i).GetComponent<Animator>();
            sprite.SetInteger("particleAnimation", 2);
        }

        for (int i = 17; i < 21; i++) {
            Animator sprite = canvas.transform.GetChild(i).GetComponent<Animator>();
            sprite.SetInteger("particleAnimation", 5);
        }

        for (int i = 21; i < 25; i++) {
            Animator sprite = canvas.transform.GetChild(i).GetComponent<Animator>();
            sprite.SetInteger("particleAnimation", 3);
        }
    }
}