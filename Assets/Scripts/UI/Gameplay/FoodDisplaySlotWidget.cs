using UnityEngine;
using UnityEngine.UI;

public class FoodDisplaySlotWidget : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private GestureSlotWidget gestureSlot;

    public void SetData(FoodLineUp f)
    {
        icon.sprite = f.Food.Sprite;
        gestureSlot.SetData(f.Gesture);
    }
}