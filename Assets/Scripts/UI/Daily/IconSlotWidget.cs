using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconSlotWidget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amountLabel;
    [SerializeField] private Image iconImage;

    public void SetData(IItemReward item)
    {
        amountLabel.text = item.Amount.ToString();
        iconImage.sprite = item.Data.Sprite;
    }
}