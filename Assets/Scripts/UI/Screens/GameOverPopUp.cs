using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopUp : ScreenBase
{
    [SerializeField] private TextMeshProUGUI coinAmount;
    [SerializeField] private Button nextButton;

    private void Start()
    {
        nextButton.onClick.AddListener(OnClickNext);
    }

    private void OnClickNext()
    {
        Hide(true);
    }

    internal void SetData(int finalScore)
    {
        int coins = finalScore / 100;

        PlayerData.ClaimReward(new ItemReward(coins, new("COIN", null)));
        coinAmount.text = $"+{coins}";
    }
}