using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopUp : ScreenBase
{
    [SerializeField] Button nextButton;

    private void Start()
    {
        nextButton.onClick.AddListener(OnClickNext);
    }

    private void OnClickNext()
    {
        Hide(true);
    }
}
