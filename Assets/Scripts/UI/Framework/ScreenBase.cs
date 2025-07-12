using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(GraphicRaycaster))]
public abstract class ScreenBase : MonoBehaviour, IScreen
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private GraphicRaycaster raycaster;

    public Canvas Canvas => canvas;
    public CanvasGroup CanvasGroup => canvasGroup;
    public GraphicRaycaster Raycaster => raycaster;

    public event IScreen.ScreenCallback OnShowStart;

    public event IScreen.ScreenCallback OnShowComplete;

    public event IScreen.ScreenCallback OnHideStart;

    public event IScreen.ScreenCallback OnHideComplete;

    protected virtual bool AllowBack => true;

    private IScreenShowTransitioner showTransitioner;
    private IScreenFocusTransitioner focusTransitioner;
    private IScreenHideTransitioner hideTransitioner;

    protected virtual void OnShowTransitionStart()
    { }

    protected virtual void OnHideTransitionStart()
    { }

    protected virtual void OnFocusTransitionStart()
    { }

    protected virtual void OnUnfocusTransitionStart()
    { }

    protected virtual void OnShowTransitionCompleted()
    { }

    protected virtual void OnHideTransitionCompleted()
    { }

    protected virtual void OnFocusTransitionCompleted()
    { }

    protected virtual void OnUnfocusTransitionCompleted()
    { }

    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        raycaster = GetComponent<GraphicRaycaster>();

        if (!TryGetComponent(out showTransitioner))
        {
            showTransitioner = new NullScreenShowTransitioner(this);
        }
        if (!TryGetComponent(out hideTransitioner))
        {
            hideTransitioner = new NullScreenHideTransitioner(this);
        }
        if (!TryGetComponent(out focusTransitioner))
        {
            focusTransitioner = new NullScreenFocusTransitioner(this);
        }

        canvas.enabled = false;
    }

    void IScreen.Show(bool instant)
    {
        OnShowStart?.Invoke(this);
        OnShowTransitionStart();

        if (instant)
        {
            canvas.enabled = true;
            canvasGroup.alpha = 1.0f;
            raycaster.enabled = true;
            Show_CompletedCallback();
        }
        else
        {
            showTransitioner.Run(Show_CompletedCallback);
        }
    }

    void IScreen.Hide(bool instant)
    {
        OnHideStart?.Invoke(this);
        OnHideTransitionStart();

        if (instant)
        {
            canvas.enabled = false;
            canvasGroup.alpha = 0.0f;
            raycaster.enabled = false;
            Hide_CompletedCallback();
        }
        else
        {
            hideTransitioner.Run(Hide_CompletedCallback);
        }
    }

    protected void Hide(bool instant = false)
    {
        (this as IScreen).Hide();
    }

    void IScreen.Focus()
    {
        OnFocusTransitionStart();
        focusTransitioner.Focus(Focus_CompletedCallback);
    }

    void IScreen.Unfocus()
    {
        OnUnfocusTransitionStart();
        focusTransitioner.Unfocus(Unfocus_CompletedCallback);
    }

    bool IScreen.TryHide()
    {
        if (AllowBack)
        {
            Hide();
            return true;
        }

        return false;
    }

    private void Show_CompletedCallback()
    {
        OnShowComplete?.Invoke(this);
        OnShowTransitionCompleted();
    }

    private void Hide_CompletedCallback()
    {
        OnHideComplete?.Invoke(this);
        OnHideTransitionCompleted();
    }

    private void Focus_CompletedCallback()
    {
        OnFocusTransitionCompleted();
    }

    private void Unfocus_CompletedCallback()
    {
        OnUnfocusTransitionCompleted();
    }
}