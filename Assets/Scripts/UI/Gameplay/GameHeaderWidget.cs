using System.Collections.Generic;
using UnityEngine;

public class GameHeaderWidget : MonoBehaviour
{
    [SerializeField] private List<FoodDisplaySlotWidget> foodWidgets;
    [SerializeField] private BiggieWooshWidget biggieWooshWidget;

    public void UpdateHeader(List<FoodLineUp> lineUp, bool first)
    {
        if (first)
        {
            for (int i = 0; i < lineUp.Count; i++)
            {
                foodWidgets[i].SetData(lineUp[i]);
            }
            return;
        }

        biggieWooshWidget.Play(() =>
        {
            for (int i = 0; i < lineUp.Count; i++)
            {
                foodWidgets[i].SetData(lineUp[i]);
            }
        });
    }
}