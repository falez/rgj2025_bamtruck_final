using System;
using UnityEngine;

public interface IItem
{
    public string Id { get; }
    public Sprite Sprite { get; }
}

public interface IItemReward
{
    public IItem Data { get; }
    public int Amount { get; }
}

[Serializable]
public struct ItemData : IItem
{
    [SerializeField] private string id;
    [SerializeField] private Sprite sprite;

    public readonly string Id => id;

    public readonly Sprite Sprite => sprite;

    public ItemData(string id, Sprite sprite)
    {
        this.id = id;
        this.sprite = sprite;
    }
}

[Serializable]
public struct ItemReward : IItemReward
{
    [SerializeField] private int amount;
    [SerializeField] private ItemData data;
    public readonly IItem Data => data;
    public readonly int Amount => amount;

    public ItemReward(int amount, ItemData data)
    {
        this.amount = amount;
        this.data = data;
    }
}

[Serializable]
public struct DailyReward : IItemReward
{
    [SerializeField] private ItemDataSO data;
    [SerializeField] private Sprite iconOverride;
    [SerializeField] private int amount;

    public readonly IItem Data => data;
    public readonly int Amount => amount;

    public readonly Sprite Sprite => iconOverride != null ? iconOverride : data.Sprite;
}