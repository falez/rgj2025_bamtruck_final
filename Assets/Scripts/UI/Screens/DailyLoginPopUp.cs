using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyLoginPopUp : ScreenBase
{
    [SerializeField] private ClaimRewardPopUp rewardClaimPopUp;

    [SerializeField] private Button claimButton;
    [SerializeField] private RectTransform content;

    private List<DailySlotWidget> dailyWidgets = new List<DailySlotWidget>();

    private DailyRewardCalendar calendarData;

    private void Start()
    {
        calendarData = DailySystem.DailyCalendar;

        claimButton.onClick.AddListener(OnClickClaim);

        content.GetComponentsInChildren(dailyWidgets);

        for (int i = 0; i < calendarData.Rewards.Count; i++)
        {
            dailyWidgets[i].SetData(calendarData.Rewards[i], i + 1);
        }
    }

    protected override void OnShowTransitionStart()
    {
        int dayLogged = PlayerData.DaysLogged;

        for (int i = 0; i < calendarData.Rewards.Count; i++)
        {
            DailySlotWidget.State state;

            if (i < dayLogged)
            {
                state = DailySlotWidget.State.CLAIMED;
            }
            else if (i == dayLogged)
            {
                state = DailySlotWidget.State.HIGHLIGHT;
            }
            else
            {
                state = DailySlotWidget.State.NONE;
            }

            dailyWidgets[i].SetState(state);
        }
    }

    private void OnClickClaim()
    {
        var rwd = DailySystem.ClaimReward();
        Hide(true);

        rewardClaimPopUp.SetData(new() { rwd });
        ScreenManager.ShowOverlay(rewardClaimPopUp, true);
    }
}