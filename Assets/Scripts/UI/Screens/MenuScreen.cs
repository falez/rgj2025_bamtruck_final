using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : ScreenBase
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;

    [SerializeField] private SettingsScreen settingsScreen;
    [SerializeField] private GameplayScreen gameScreen;

    private MenuFocusTransitioner focusTransitioner;

    protected override bool AllowBack => false;

    protected override void Awake()
    {
        base.Awake();

        focusTransitioner = GetComponent<MenuFocusTransitioner>();
    }

    private void Start()
    {
        playButton.onClick.AddListener(OnClickPlay);
        settingsButton.onClick.AddListener(OnClickSettings);
    }

    private void OnClickPlay()
    {
        focusTransitioner.UnfocusIsHide = true;
        ScreenManager.Show(gameScreen);
    }

    private void OnClickSettings()
    {
        focusTransitioner.UnfocusIsHide = false;
        ScreenManager.Show(settingsScreen);
    }

    protected override void OnShowTransitionCompleted()
    {
        focusTransitioner.UnfocusIsHide = false;
    }
}