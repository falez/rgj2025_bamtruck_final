using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsScreen : PopUpBase
{
    [SerializeField] private Button resetButton;

    private void Start()
    {
        resetButton.onClick.AddListener(() =>
        {
            PlayerData.Clear();
            SceneManager.LoadScene("MenuScene");
        });
    }
}