using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameEndManager : BaseController {
    public static GameEndManager Instance;

    private GameObject prefab;
    private GameObject endDisplay;
    private GameObject rewardsButton;
    private GameObject cardRewardCanvas;

    private bool battleOver;

    Dictionary<int, Card> rewardCards;

    protected override bool Initialize(bool reinitialize) {
        Instance = this;

        if (VisualController.Instance != null && VisualController.Instance.Initialized &&
            RewardController.Instance != null && RewardController.Instance.Initialized &&
            CardManager.Instance != null && CardManager.Instance.Initialized) {
            battleOver = false;
            prefab = VisualController.Instance.GetPrefab("Rewards");

            rewardCards = new Dictionary<int, Card>();

            // Set up listener for the end of battle
            EndBattle.OnEndBattle += ShowGameEnd;

            return true;
        }

        return false;
    }

    public void ShowGameEnd(int turn, bool win) {
        EndBattle.OnEndBattle -= ShowGameEnd;
        if (!battleOver) {
            battleOver = true;

            endDisplay = Object.Instantiate(prefab);
            endDisplay.SetActive(true);

            // Move the visual to the front
            endDisplay.transform.SetAsLastSibling();
            endDisplay.transform.position = Vector3.zero;
            endDisplay.GetComponent<Canvas>().worldCamera = VisualController.Instance.mainCamera;
            Image background = endDisplay.transform.GetChild(0).GetComponent<Image>();
            // TextMeshPro text = canvas.transform.GetChild(canvas.transform.childCount - 1).GetComponent<TextMeshPro>();
            endDisplay.transform.localScale = Vector3.one;

            rewardsButton = endDisplay.transform.GetChild(1).gameObject;
            cardRewardCanvas = endDisplay.transform.GetChild(2).gameObject;

            // Create card rewards
            rewardCards.Clear();
            for (int i = 0; i < 3; i++) {
                Card rewardCard = new Card(CardManager.Instance.GenerateRewardCard(rewardCards.Select(kv => kv.Value).ToDictionary(card => card.name)));
                rewardCard.CreateDudVisual(cardRewardCanvas.transform, 1.3f);
                rewardCards.Add(rewardCard.Id, rewardCard);
            }

            rewardsButton.SetActive(false);
            cardRewardCanvas.SetActive(false);

            // Fade the background in and show button on fade complete
            LeanTween.value(0, 0.65f, 2f).setOnUpdate((float alpha) => {
                background.color = new Color(0.55f, 0.55f, 0.55f, alpha);
            }).setOnComplete(() => {
                rewardsButton.SetActive(true);
                RewardsButton.OnRewardsButtonClicked += ShowCardRewards;
            });
        }
    }

    public void ShowCardRewards() {
        RewardsButton.OnRewardsButtonClicked -= ShowCardRewards;
        rewardsButton.SetActive(false);
        cardRewardCanvas.SetActive(true);
        LeanTween.value(0, 1.3f, 0.5f).setOnUpdate((float scale) => {
            foreach (KeyValuePair<int, Card> cardPair in rewardCards) {
                cardPair.Value.SetVisualScale(new Vector3(scale, scale, 1));
            }
        }).setOnComplete(() => {
            CardSelect.OnCardSelect += AddCardToRunDeck;
        });
    }

    private void AddCardToRunDeck(int cardId) {
        CardSelect.OnCardSelect -= AddCardToRunDeck;
        // Add the card to the run deck and load the next battle
        if (rewardCards.TryGetValue(cardId, out Card chosenCard)) {
            CardManager.Instance.AddCardToRunDeck(chosenCard);
            ResourceController.Instance.LoadNextLevel();
        }
    }
}
