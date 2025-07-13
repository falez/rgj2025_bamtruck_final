using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScreen : ScreenBase
{
    [SerializeField] private GameOverPopUp gameOverPopUp;

    [SerializeField] private Button pauseButton;

    [SerializeField] private BamTruckGameplayController gameplayController;

    [SerializeField] private GameObject bubble;
    [SerializeField] private CustomerSwitchingWidget customerWidget;
    [SerializeField] private OrderBubbleWidget orderBubbleWidget;
    [SerializeField] private GameHeaderWidget gameHeaderWidget;

    [SerializeField] private GameStartWidget gameStartWidget;
    [SerializeField] private GameOverWidget gameOverWidget;

    [SerializeField] private Animation bamAnimation;

    [SerializeField] private GestureRecog gestureRecognizer;

    [SerializeField] private TextMeshProUGUI debugStateLabel;
    [SerializeField] private TextMeshProUGUI debugGestureLabel;

    private GameCustomer currentCustomer;

    private bool gameIsOver = false;

    protected override bool AllowBack => false;

    private void Start()
    {
        pauseButton.onClick.AddListener(OnClickPause);

        gameplayController.OnRoundComplete += GameplayController_OnRoundComplete;
        gameplayController.OnCustomerChanged += GameplayController_OnCustomerChanged;
        gameplayController.OnGameEnded += GameplayController_OnGameEnded;
        gameplayController.OnCustomerOrderWrong += GameplayController_OnCustomerOrderWrong;
        gameplayController.OnCustomerOrderCorrect += GameplayController_OnCustomerOrderCorrect;

        gameplayController.OnFoodLineUpChanged += GameplayController_OnFoodLineUpChanged;

        gestureRecognizer.onGestureMade += GestureRecognizer_onGestureMade;
    }

    private void GameplayController_OnFoodLineUpChanged(List<FoodLineUp> list, bool first)
    {
        gameHeaderWidget.UpdateHeader(list);
    }

    private void GameplayController_OnCustomerOrderCorrect(int index)
    {
        bamAnimation.Play("PushFood");
        orderBubbleWidget.TickFood(index);
        customerWidget.Boing(0.5f);
    }

    private void GameplayController_OnCustomerOrderWrong(int index)
    {
        bamAnimation.Play("PushFood");
        customerWidget.Shake(0.5f);
    }

    private void GameplayController_OnRoundComplete(bool success, int score, int totalScore)
    {
        gestureRecognizer.AllowRecognition = false;
        gameplayController.Pause = true;

        if (success)
        {
            customerWidget.Boing(0.5f, () =>
            {
                bubble.SetActive(false);
            });
        }
        /*
        else
        {
            customerWidget.Shake(0.5f);
            bubble.SetActive(false);
        }
        */
    }

    private void GameplayController_OnGameEnded()
    {
        gestureRecognizer.AllowRecognition = false;
        bubble.SetActive(false);
        gameIsOver = true;

        gameOverWidget.Play(() =>
        {
            ScreenManager.Show(gameOverPopUp);
        });

        /*
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1.0f);
        seq.OnComplete(() =>
        {
            ScreenManager.Show(gameOverPopUp);
        });
        seq.Play();
        */
    }

    protected override void OnFocusTransitionCompleted()
    {
        if (gameIsOver)
        {
            Hide(true);
        }
    }

    protected override void OnShowTransitionStart()
    {
        bubble.SetActive(false);
        customerWidget.ResetStates();
        gameplayController.InitializeValues();
    }

    protected override void OnShowTransitionCompleted()
    {
        gameIsOver = false;

        gameStartWidget.Play(gameplayController.StartGame);
    }

    private void Update()
    {
        debugStateLabel.text = $"Score:\n{gameplayController.Score}";
    }

    private void GestureRecognizer_onGestureMade(string gestureName, float score)
    {
        debugGestureLabel.text = $"{gestureName}\n({score})";
        gameplayController.Answer(gestureName);
    }

    private void GameplayController_OnCustomerChanged(GameCustomer next)
    {
        gestureRecognizer.AllowRecognition = false;
        customerWidget.Next(OnCustomerChanged);
        currentCustomer = next;
    }

    private void OnCustomerChanged()
    {
        gestureRecognizer.AllowRecognition = true;
        gameplayController.Pause = false;
        orderBubbleWidget.Populate(currentCustomer.Orders);
        bubble.SetActive(true);
    }

    private void OnClickPause()
    {
        Hide();
    }
}