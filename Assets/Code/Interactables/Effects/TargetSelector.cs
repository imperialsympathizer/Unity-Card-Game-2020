using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class TargetSelector {
    private static GameObject targetCanvas;
    private static GameObject shadow;
    private static TextMeshProUGUI targetDialogue;

    public static void Initialize() {
        targetCanvas = GameObject.Find("TargetingCanvas");
        targetCanvas.SetActive(false);
        shadow = targetCanvas.transform.GetChild(0).gameObject;
        targetDialogue = targetCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public static void EnableTargeting(List<Target> validTargets, string dialogue = "SELECT A TARGET") {
        targetDialogue.text = dialogue.ToUpper();
        targetCanvas.SetActive(true);

        for (int i = 0; i < validTargets.Count; i++) {
            // for each valid type, enable glow around them
        }
    }

    public static void DisableTargetCanvas() {
        targetCanvas.SetActive(false);
    }
}
