using UnityEngine;
using UnityEngine.UI;

public class FoodWidget : MonoBehaviour
{
    [SerializeField] Image foodImage;
    [SerializeField] GameObject tick;

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
