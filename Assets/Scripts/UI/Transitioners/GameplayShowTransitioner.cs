using System;
using DG.Tweening;
using UnityEngine;

public class GameplayShowTransitioner : MonoBehaviour, IScreenShowTransitioner
{
    [SerializeField] private CanvasGroup transitionCanvas;
    [SerializeField] private CanvasGroup gameCanvas;
    [SerializeField] private RectTransform truck;
    [SerializeField] private RectTransform start;
    [SerializeField] private RectTransform end;

    [SerializeField] private float duration = 0.25f;

    private ScreenBase screen;
    private float halfDuration;

    private void Awake()
    {
        this.screen = GetComponent<ScreenBase>();
        halfDuration = duration * 0.5f;
    }

    public void Run(Action onComplete)
    {
        screen.Canvas.enabled = true;
        screen.CanvasGroup.alpha = 1.0f;
        screen.Raycaster.enabled = false;
        gameCanvas.alpha = 0.0f;

        truck.anchoredPosition = start.anchoredPosition;
        transitionCanvas.alpha = 1.0f;

        Sequence seq = DOTween.Sequence();

        seq.Append(truck.DOAnchorPosX(0.0f, halfDuration).SetEase(Ease.Linear));

        // maybe not???
        seq.AppendCallback(() =>
        {
            gameCanvas.alpha = 1.0f;
        });

        seq.Append(truck.DOAnchorPosX(end.anchoredPosition.x, halfDuration).SetEase(Ease.Linear));

        seq.OnComplete(() =>
        {
            transitionCanvas.alpha = 0.0f;
            screen.Raycaster.enabled = true;
            onComplete?.Invoke();
        });

        seq.Play();
    }
}