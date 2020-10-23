using System;
using System.Collections.Generic;
using TMPro;

public class NumberAnimator : BaseController {
    public static NumberAnimator Instance;

    private List<Tuple<TextMeshProUGUI, LTDescr>> currentUIAnimations = new List<Tuple<TextMeshProUGUI, LTDescr>>();

    // TODO: add to global variable
    private float timeConstant = 0.5f;

    protected override bool Initialize() {
        Instance = this;
        return true;
    }

    public void AnimateNumberChange(TextMeshProUGUI textUGUI, int finalNum) {
        // Check to make sure no other animations are running
        // If so, cancel them
        for (int i = 0; i < currentUIAnimations.Count; i++) {
            if (currentUIAnimations[i].Item1 == textUGUI) {
                LeanTween.cancel(currentUIAnimations[i].Item2.id);
                currentUIAnimations.RemoveAt(i);
            }
        }
        int startNum = Int32.Parse(textUGUI.text);

        float time = Math.Abs((finalNum - startNum) * timeConstant);

        LTDescr description = LeanTween.value(startNum, finalNum, timeConstant).setOnUpdate((float val) => {
            textUGUI.text = ((int)val).ToString();
        });

        currentUIAnimations.Add(new Tuple<TextMeshProUGUI, LTDescr>(textUGUI, description));
    }

    public void AnimateNumberChange(TextMeshPro textTMP, int startNum, int finalNum) {
        float time = Math.Abs((finalNum - startNum) * timeConstant);

        LeanTween.value(startNum, finalNum, time).setOnUpdate((float val) => {
            textTMP.text = ((int)val).ToString();
        });
    }
}
