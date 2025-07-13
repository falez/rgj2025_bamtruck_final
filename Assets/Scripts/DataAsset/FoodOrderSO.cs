using UnityEngine;

[CreateAssetMenu(fileName = "New GameFood", menuName = "Bamzaar/Game/Food")]
public class FoodOrderSO : ScriptableObject, IFoodOrder
{
    [SerializeField] private Sprite sprite;

    public string Id => name;

    public Sprite Sprite => sprite;
}