using System;
using UnityEngine;

public class MenuFocusTransitioner : MonoBehaviour, IScreenFocusTransitioner
{
    public bool UnfocusIsHide { get; set; }

    private ScreenBase screen;

    private void Awake()
    {
        this.screen = GetComponent<ScreenBase>();
    }

    public void Focus(Action onComplete)
    {
        if (UnfocusIsHide)
        {
            screen.Canvas.enabled = true;
            screen.CanvasGroup.alpha = 1.0f;
        }

        screen.Raycaster.enabled = true;
        onComplete?.Invoke();
    }

    public void Unfocus(Action onComplete)
    {
        if (UnfocusIsHide)
        {
            screen.Canvas.enabled = false;
            screen.CanvasGroup.alpha = 0.0f;
        }

        screen.Raycaster.enabled = false;
        onComplete?.Invoke();
    }
}