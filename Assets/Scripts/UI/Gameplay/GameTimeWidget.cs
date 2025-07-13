using UnityEngine;
using UnityEngine.UI;

public class GameTimeWidget : MonoBehaviour
{
    [SerializeField] private BamTruckGameplayController gameplayController;
    [SerializeField] private Slider slider;

    private void Update()
    {
        slider.value = gameplayController.NormalizedDuration;
    }
}