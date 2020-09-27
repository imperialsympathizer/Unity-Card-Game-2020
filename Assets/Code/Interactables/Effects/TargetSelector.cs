using TMPro;
using UnityEngine;

public class TargetSelector {
    public static TargetSelector SharedInstance;

    private GameObject targetCanvas;
    private GameObject shadow;
    private TextMeshProUGUI targetDialogue;

    public void Initialize() {
        SharedInstance = this;

        targetCanvas = GameObject.Find("TargetingCanvas");
        targetCanvas.SetActive(false);
        shadow = targetCanvas.transform.GetChild(0).gameObject;
        targetDialogue = targetCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void EnableTargetCanvas(string dialogue = "SELECT A TARGET") {
        targetDialogue.text = dialogue.ToUpper();
        targetCanvas.SetActive(true);
    }

    public void DisableTargetCanvas() {
        targetCanvas.SetActive(false);
    }
}
