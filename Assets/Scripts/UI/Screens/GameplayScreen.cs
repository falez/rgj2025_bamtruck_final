using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScreen : ScreenBase
{
    [SerializeField] private Button pauseButton;

    [SerializeField] private BamTruckGameplayController gameplayController;

    [SerializeField] private Button triangleButton;
    [SerializeField] private Button squareButton;
    [SerializeField] private Button circleButton;

    [SerializeField] GameObject bubble;
    [SerializeField] private CustomerWidget customerWidget;

    protected override bool AllowBack => false;

    private bool allowAnswer = false;

    private void Start()
    {
        pauseButton.onClick.AddListener(OnClickPause);

        triangleButton.onClick.AddListener(OnAnswerTriangle);
        squareButton.onClick.AddListener(OnAnswerSquare);
        circleButton.onClick.AddListener(OnAnswerCircle);

        gameplayController.OnRoundComplete += GameplayController_OnRoundComplete;
        gameplayController.OnCustomerChanged += GameplayController_OnCustomerChanged;
        bubble.SetActive(false);
    }

    private void GameplayController_OnRoundComplete(bool success, int score)
    {
        allowAnswer = false;
        bubble.SetActive(false);
    }

    private void GameplayController_OnCustomerChanged(GameCustomer next)
    {
        allowAnswer = false;
        customerWidget.Next(OnCustomerChanged);
    }

    private void OnCustomerChanged()
    {
        allowAnswer = true;
        bubble.SetActive(true);
    }

    private void OnAnswerSquare()
    {
        if (!allowAnswer) return;

        gameplayController.Answer("Square");
    }

    private void OnAnswerTriangle()
    {
        if (!allowAnswer) return;

        gameplayController.Answer("Triangle");
    }
    private void OnAnswerCircle()
    {
        if (!allowAnswer) return;

        gameplayController.Answer("Circle");
    }


    private void OnClickPause()
    {
        Hide();
    }

    protected override void OnShowTransitionStart()
    {
        customerWidget.ResetStates();
    }

    protected override void OnShowTransitionCompleted()
    {
        gameplayController.StartGame();
    }
}