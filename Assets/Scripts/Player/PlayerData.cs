using System;
using UnityEngine;

public static class PlayerData
{
    public static int DaysLogged { get; private set; } = 0;
    public static DateTime LastLoggedDate { get; private set; }

    static PlayerData()
    {
        DaysLogged = PlayerPrefs.GetInt("DaysLogged", 0);

        string tickStr = PlayerPrefs.GetString("LastLoggedDate", "0");
        long ticks = Convert.ToInt64(tickStr);

        LastLoggedDate = new DateTime(ticks).Date;
    }

    public static void ClaimReward(IItemReward rwd)
    {
        Debug.LogWarning("TODO: Claim rewards");
    }

    public static void Clear()
    {
        PlayerPrefs.DeleteAll();

        DaysLogged = PlayerPrefs.GetInt("DaysLogged", 0);

        string tickStr = PlayerPrefs.GetString("LastLoggedDate", "0");
        long ticks = Convert.ToInt64(tickStr);

        LastLoggedDate = new DateTime(ticks).Date;
    }

    public static void UpdateDailyDetails(bool reset)
    {
        DaysLogged = reset ? 0 : (DaysLogged + 1);
        LastLoggedDate = DateTime.UtcNow.Date;
        PlayerPrefs.SetInt("DaysLogged", DaysLogged);
        PlayerPrefs.SetString("LastLoggedDate", LastLoggedDate.Ticks.ToString());
    }
}