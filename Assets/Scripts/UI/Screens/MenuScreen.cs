using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScreen : ScreenBase
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button upgradeButton;

    [SerializeField] private ShopPopUp shopPopUp;
    [SerializeField] private DailyLoginPopUp dailyPopUp;
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
        upgradeButton.onClick.AddListener(OnClickUpgrades);
    }

    private void OnClickUpgrades()
    {
        //PlayerData.Clear();
        //SceneManager.LoadScene("MenuScene");
        ScreenManager.Show(shopPopUp);
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

    protected override void OnFocusTransitionCompleted()
    {
        if (DailySystem.CheckDailyLogin())
            ScreenManager.Show(dailyPopUp);
    }

    protected override void OnShowTransitionCompleted()
    {
        focusTransitioner.UnfocusIsHide = false;

        if (DailySystem.CheckDailyLogin())
            ScreenManager.Show(dailyPopUp);
    }
}