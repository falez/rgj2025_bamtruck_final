using System;
using DG.Tweening;
using UnityEngine;

public interface IScreenShowHandle
{
    void Run(System.Action onComplete);
}

public class NullScreenShowHandle : IScreenShowHandle
{
    private ScreenBase screen;

    public NullScreenShowHandle(ScreenBase screen)
    {
        this.screen = screen;
    }

    public void Run(Action onComplete)
    {
        screen.Canvas.enabled = true;
        screen.CanvasGroup.alpha = 1.0f;
        screen.Raycaster.enabled = true;
        onComplete?.Invoke();
    }
}

public class ScreenShowHandler : MonoBehaviour, IScreenShowHandle
{
    private ScreenBase screen;

    private void Awake()
    {
        this.screen = GetComponent<ScreenBase>();
    }

    public void Run(Action onComplete)
    {
        screen.Canvas.enabled = true;
        screen.CanvasGroup.alpha = 0.0f;
        screen.Raycaster.enabled = true;

        Sequence seq = DOTween.Sequence();

        seq.Append(screen.CanvasGroup.DOFade(1.0f, 0.25f));
        seq.OnComplete(() =>
        {
            screen.Raycaster.enabled = true;
            onComplete?.Invoke();
        });

        seq.Play();
    }
}