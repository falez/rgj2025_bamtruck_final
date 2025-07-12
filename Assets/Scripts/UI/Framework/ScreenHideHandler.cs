using System;
using DG.Tweening;
using UnityEngine;

public interface IScreenHideHandle
{
    void Run(System.Action onComplete);
}

public class NullScreenHideHandle : IScreenHideHandle
{
    private ScreenBase screen;

    public NullScreenHideHandle(ScreenBase screen)
    {
        this.screen = screen;
    }

    public void Run(Action onComplete)
    {
        screen.Canvas.enabled = false;
        screen.CanvasGroup.alpha = 0.0f;
        screen.Raycaster.enabled = false;
        onComplete?.Invoke();
    }
}

public class ScreenHideHandler : MonoBehaviour, IScreenHideHandle
{
    private ScreenBase screen;

    private void Awake()
    {
        this.screen = GetComponent<ScreenBase>();
    }

    public void Run(Action onComplete)
    {
        screen.Canvas.enabled = true;
        screen.CanvasGroup.alpha = 1.0f;
        screen.Raycaster.enabled = false;

        Sequence seq = DOTween.Sequence();

        seq.Append(screen.CanvasGroup.DOFade(0.0f, 0.25f));
        seq.OnComplete(() =>
        {
            screen.Canvas.enabled = false;
            onComplete?.Invoke();
        });

        seq.Play();
    }
}