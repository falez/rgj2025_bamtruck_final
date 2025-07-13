using UnityEngine;
using UnityEngine.UI;

public class GestureSlotWidget : MonoBehaviour
{
    [SerializeField] private Image icon;

    public void SetData(GameGesture gesture)
    {
        icon.sprite = gesture.Sprite;
    }
}