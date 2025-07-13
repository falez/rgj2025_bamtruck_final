using UnityEngine;
using UnityEngine.UI;

public class FoodOrderWidget : MonoBehaviour
{
    [SerializeField] private Image foodImage;
    [SerializeField] private GameObject tick;

    public void SetData(Sprite foodSprite)
    {
        foodImage.sprite = foodSprite;
        EnableTick(false);
    }

    public void EnableTick(bool enable)
    {
        tick.SetActive(enable);
    }
}