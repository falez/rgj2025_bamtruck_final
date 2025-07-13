using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsScreen : PopUpBase
{
    [SerializeField] private Button resetButton;

    protected override void OnStart()
    {
        resetButton.onClick.AddListener(() =>
        {
            PlayerData.Clear();
            SceneManager.LoadScene("MenuScene");
        });
    }
}