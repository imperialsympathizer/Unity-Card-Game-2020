using System;
using UnityEngine;
using UnityEngine.UI;

public static class AttackAnimator {
    // This class creates a slow backward slide then fast forward slide to simulate an "attack" by a character, reseting the character's position once complete

    private static HorizontalLayoutGroup playerCanvas;
    private static HorizontalLayoutGroup summonCanvas;
    private static HorizontalLayoutGroup enemyCanvas;

    public static event Action OnAnimateComplete;

    public static void Initialize() {
        playerCanvas = GameObject.Find("Players").GetComponent<HorizontalLayoutGroup>();
        summonCanvas = GameObject.Find("Summons").GetComponent<HorizontalLayoutGroup>();
        enemyCanvas = GameObject.Find("Enemies").GetComponent<HorizontalLayoutGroup>();
    }

    public static void AnimateAttack(GameObject character, Fighter.FighterType type) {
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

    private static void EnableCanvasGrid(Fighter.FighterType type, bool enable) {
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
