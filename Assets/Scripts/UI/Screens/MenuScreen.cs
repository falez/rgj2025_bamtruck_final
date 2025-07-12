using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuScreen : ScreenBase
{
    [SerializeField] private Button settingsButton;

    [SerializeField] private SettingsScreen settingsScreen;

    protected override bool AllowBack => false;

    private void Start()
    {
        settingsButton.onClick.AddListener(OnClickSettings);
    }

    private void OnClickSettings()
    {
        ScreenManager.Show(settingsScreen);
    }
}