using System;
using UnityEngine;
using UnityEngine.UI;

public class AttackAnimator : BaseController {
    // This class creates a slow backward slide then fast forward slide to simulate an "attack" by a character, reseting the character's position once complete
    public static AttackAnimator Instance;

    private HorizontalLayoutGroup playerCanvas;
    private HorizontalLayoutGroup summonCanvas;
    private HorizontalLayoutGroup enemyCanvas;

    public static event Action OnAnimateComplete;

    protected override bool Initialize(bool reinitialize) {
        Instance = this;

        if (VisualController.Instance != null && VisualController.Instance.Initialized) {
            playerCanvas = VisualController.Instance.GetPlayerCanvas().GetComponent<HorizontalLayoutGroup>();
            summonCanvas = VisualController.Instance.GetSummonCanvas().GetComponent<HorizontalLayoutGroup>();
            enemyCanvas = VisualController.Instance.GetEnemyCanvas().GetComponent<HorizontalLayoutGroup>();

            return true;
        }

        return false;
    }

    public void AnimateAttack(GameObject character, Fighter.FighterType type) {
        float moveAmount = 50f;
        float backTime = 0.2f;
        float forwardTime = 0.1f;
        float resetTime = 0.2f;
        if (type == Fighter.FighterType.PLAYER || type == Fighter.FighterType.SUMMON) {
            moveAmount *= -1;
        }
        EnableCanvasGrid(type, false);
        LeanTween.moveLocalX(character, character.transform.localPosition.x + moveAmount, backTime).setOnComplete(() => {
            LeanTween.moveLocalX(character, character.transform.localPosition.x - moveAmount * 2, forwardTime).setOnComplete(() => {
                LeanTween.moveLocalX(character, character.transform.localPosition.x + moveAmount, resetTime).setOnComplete(() => {
                    EnableCanvasGrid(type, true);
                    OnAnimateComplete?.Invoke();
                });
            });
        });
    }

    private void EnableCanvasGrid(Fighter.FighterType type, bool enable) {
        switch (type) {
            case Fighter.FighterType.PLAYER:
                playerCanvas.enabled = enable;
                break;
            case Fighter.FighterType.SUMMON:
                summonCanvas.enabled = enable;
                break;
            case Fighter.FighterType.ENEMY:
                enemyCanvas.enabled = enable;
                break;
        }
    }
}
