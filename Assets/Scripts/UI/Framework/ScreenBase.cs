using Unity.Burst.CompilerServices;
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

    private IScreenShowHandle showHandle;
    private IScreenHideHandle hideHandle;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        raycaster = GetComponent<GraphicRaycaster>();

        if (!TryGetComponent(out showHandle))
        {
            showHandle = new NullScreenShowHandle(this);
        }
        if (!TryGetComponent(out hideHandle))
        {
            hideHandle = new NullScreenHideHandle(this);
        }

        canvas.enabled = false;
    }

    void IScreen.Show(bool instant)
    {
        OnShowStart?.Invoke(this);

        if (instant)
        {
            canvas.enabled = true;
            canvasGroup.alpha = 1.0f;
            raycaster.enabled = true;
            Show_CompletedCallback();
        }
        else
        {
            showHandle.Run(Show_CompletedCallback);
        }
    }

    void IScreen.Hide(bool instant)
    {
        OnHideStart?.Invoke(this);
        if (instant)
        {
            canvas.enabled = false;
            canvasGroup.alpha = 0.0f;
            raycaster.enabled = false;
            Hide_CompletedCallback();
        }
        else
        {
            hideHandle.Run(Hide_CompletedCallback);
        }
    }

    protected void Hide(bool instant = false)
    {
        (this as IScreen).Hide();
    }

    void IScreen.Focus()
    {
        raycaster.enabled = true;
    }

    void IScreen.Unfocus()
    {
        raycaster.enabled = false;
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
    }

    private void Hide_CompletedCallback()
    {
        OnHideComplete?.Invoke(this);
    }
}