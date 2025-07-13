using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IFoodOrder
{
    Sprite Sprite { get; }
    string Id { get; }
}

[Serializable]
public struct FoodOrder : IFoodOrder
{
    public Sprite sprite;
    public string id;

    public Sprite Sprite => sprite;

    public string Id => id;

    public override string ToString()
    {
        return id;
    }
}

[Serializable]
public class FoodOrderInstance : IFoodOrder
{
    public Sprite sprite;
    public string id;
    public bool met;

    public Sprite Sprite => sprite;

    public string Id => id;

    public FoodOrderInstance(IFoodOrder order)
    {
        sprite = order.Sprite;
        id = order.Id;
        met = false;
    }

    public override string ToString()
    {
        return id;
    }
}

public class OrderBubbleWidget : MonoBehaviour
{
    [SerializeField] private FoodOrderWidget foodPrefab;

    [SerializeField] private RectTransform content;

    private readonly List<FoodOrderWidget> foodWidgets = new();

    private ObjectPool<FoodOrderWidget> pool;

    private void Awake()
    {
        pool = new ObjectPool<FoodOrderWidget>(CreateFoodWidget, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 4, 8);
    }

    private FoodOrderWidget CreateFoodWidget()
    {
        return Instantiate(foodPrefab);
    }

    private void OnTakeFromPool(FoodOrderWidget widget)
    {
        widget.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(FoodOrderWidget widget)
    {
        widget.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(FoodOrderWidget widget)
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