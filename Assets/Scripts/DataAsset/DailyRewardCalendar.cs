using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DailyRewardCalendar", menuName = "Bamzaar/DailyRewardCalendar")]
public class DailyRewardCalendar : ScriptableObject
{
    [SerializeField] private List<DailyReward> rewards;

    public List<DailyReward> Rewards => rewards;
}