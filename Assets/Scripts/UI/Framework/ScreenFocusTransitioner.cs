using System;

public interface IScreenFocusTransitioner
{
    void Focus(System.Action onComplete);

    void Unfocus(System.Action onComplete);
}

public class NullScreenFocusTransitioner : IScreenFocusTransitioner
{
    private ScreenBase screen;

    public NullScreenFocusTransitioner(ScreenBase screen)
    {
        this.screen = screen;
    }

    public void Focus(Action onComplete)
    {
        screen.Canvas.enabled = true;
        screen.CanvasGroup.alpha = 1.0f;
        screen.Raycaster.enabled = true;
        onComplete?.Invoke();
    }

    public void Unfocus(Action onComplete)
    {
        screen.Canvas.enabled = true;
        screen.CanvasGroup.alpha = 1.0f;
        screen.Raycaster.enabled = false;
        onComplete?.Invoke();
    }
}