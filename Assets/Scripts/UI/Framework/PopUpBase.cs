using UnityEngine;
using UnityEngine.UI;

public class PopUpBase : ScreenBase
{
    [SerializeField] private Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(OnClickClose);
        OnStart();
    }

    protected virtual void OnStart()
    { }

    private void OnClickClose()
    {
        Hide();
    }
}