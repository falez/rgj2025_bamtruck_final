using UnityEngine;
using UnityEngine.UI;

public class GameplayScreen : ScreenBase
{
    [SerializeField] private Button pauseButton;

    protected override bool AllowBack => false;

    private void Start()
    {
        pauseButton.onClick.AddListener(OnClickPause);
    }

    private void OnClickPause()
    {
        Hide();
    }
}