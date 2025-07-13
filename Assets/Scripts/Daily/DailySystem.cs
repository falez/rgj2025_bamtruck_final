using System;
using UnityEngine;

public static class DailySystem
{
    public static DailyRewardCalendar DailyCalendar { get; private set; }

    static DailySystem()
    {
        DailyCalendar = Resources.Load<DailyRewardCalendar>("DataAsset/MockDailyCalendar");
    }

    public static bool CheckDailyLogin()
    {
        DateTime lastLogged = PlayerData.LastLoggedDate;
        //DateTime nextReward = lastLogged.AddDays(1.0f);
        //DateTime now = DateTime.UtcNow.Date;
        DateTime nextReward = lastLogged.AddSeconds(5.0f);
        DateTime now = DateTime.UtcNow;

        Debug.Log($"Last: {lastLogged}\nNext: {nextReward}\nNow: {now}");

        return now.CompareTo(nextReward) > 0;
    }

    public static DailyReward ClaimReward()
    {
        int dayLogged = PlayerData.DaysLogged;
        var reward = DailyCalendar.Rewards[dayLogged];
        PlayerData.ClaimReward(reward);
        bool needReset = dayLogged + 1 >= DailyCalendar.Rewards.Count;
        PlayerData.UpdateDailyDetails(needReset);
        return reward;
    }
}