using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IFoodOrder
{
    Sprite Sprite { get; }
    string Name { get; }
}

[Serializable]
public struct FoodOrder : IFoodOrder
{
    public Sprite sprite;
    public string name;

    public Sprite Sprite => sprite;

    public string Name => name;

    public override string ToString()
    {
        return name;
    }
}

[Serializable]
public class FoodOrderInstance : IFoodOrder
{
    public Sprite sprite;
    public string name;
    public bool met;

    public Sprite Sprite => sprite;

    public string Name => name;

    public FoodOrderInstance(IFoodOrder order)
    {
        sprite = order.Sprite;
        name = order.Name;
        met = false;
    }

    public override string ToString()
    {
        return name;
    }
}

public class OrderBubbleWidget : MonoBehaviour
{
    [SerializeField] private FoodWidget foodPrefab;

    [SerializeField] private RectTransform content;

    private readonly List<FoodWidget> foodWidgets = new();

    private ObjectPool<FoodWidget> pool;

    private void Awake()
    {
        pool = new ObjectPool<FoodWidget>(CreateFoodWidget, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 4, 8);
    }

    private FoodWidget CreateFoodWidget()
    {
        return Instantiate(foodPrefab);
    }

    private void OnTakeFromPool(FoodWidget widget)
    {
        widget.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(FoodWidget widget)
    {
        widget.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(FoodWidget widget)
    {
        Destroy(widget.gameObject);
    }

    public void Populate(List<FoodOrderInstance> foods)
    {
        foreach (var fw in foodWidgets)
        {
            pool.Release(fw);
        }

        foodWidgets.Clear();

        foreach (var food in foods)
        {
            var fw = pool.Get();
            fw.SetData(food.Sprite);

            RectTransform rt = fw.GetComponent<RectTransform>();

            rt.SetParent(content);
            rt.localScale = Vector3.one;

            foodWidgets.Add(fw);
        }
    }

    public void TickFood(int index)
    {
        if (index < 0 || index >= foodWidgets.Count)
        {
            Debug.LogWarning($"index out of range: {index}");
            return;
        }

        foodWidgets[index].EnableTick(true);
    }
}