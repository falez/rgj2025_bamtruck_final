using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class PlayerWallet
{
    [JsonProperty]
    private Dictionary<string, int> Wallet { get; set; } = new();

    public void Add(IItemReward rwd)
    {
        if (rwd.Amount <= 0) return;

        if (Wallet.ContainsKey(rwd.Data.Id))
        {
            Wallet[rwd.Data.Id] += rwd.Amount;
        }
        else
        {
            Wallet.Add(rwd.Data.Id, rwd.Amount);
        }
    }

    public bool TryConsume(string id, int amount)
    {
        if (amount <= 0) return false;

        if (Wallet.TryGetValue(id, out var inWallet))
        {
            int remainder = inWallet - amount;
            if (remainder >= 0)
            {
                Wallet[id] = remainder;
                return true;
            }
        }

        return false;
    }
}

public static class PlayerData
{
    public static int DaysLogged { get; private set; } = 0;
    public static DateTime LastLoggedDate { get; private set; }

    private static PlayerWallet wallet = new PlayerWallet();

    public static int GetValueFromWallet(string key)
    {
        return 0;
    }

    static PlayerData()
    {
        DaysLogged = PlayerPrefs.GetInt("DaysLogged", 0);

        string tickStr = PlayerPrefs.GetString("LastLoggedDate", "0");
        long ticks = Convert.ToInt64(tickStr);

        //LastLoggedDate = new DateTime(ticks).Date;
        LastLoggedDate = new DateTime(ticks);

        string json = PlayerPrefs.GetString("Wallet", "{}");
        Debug.Log(json);
        wallet = JsonConvert.DeserializeObject<PlayerWallet>(json);
    }

    public static void ClaimReward(IItemReward rwd)
    {
        wallet.Add(rwd);
        string json = JsonConvert.SerializeObject(wallet);
        Debug.Log(json);
        PlayerPrefs.SetString("Wallet", json);
    }

    public static void Clear()
    {
        PlayerPrefs.DeleteAll();

        DaysLogged = 0;
        LastLoggedDate = new DateTime(0).Date;
        wallet = new PlayerWallet();
    }

    public static void UpdateDailyDetails(bool reset)
    {
        DaysLogged = reset ? 0 : (DaysLogged + 1);
        //LastLoggedDate = DateTime.UtcNow.Date;
        LastLoggedDate = DateTime.UtcNow;
        PlayerPrefs.SetInt("DaysLogged", DaysLogged);
        PlayerPrefs.SetString("LastLoggedDate", LastLoggedDate.Ticks.ToString());
    }
}