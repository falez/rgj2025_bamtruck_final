using TMPro;
using UnityEngine;

public class DailySlotWidget : MonoBehaviour
{
    public enum State
    {
        NONE,
        CLAIMED,
        HIGHLIGHT
    }

    [SerializeField] private TextMeshProUGUI dayLabel;
    [SerializeField] private IconSlotWidget iconSlotWidget;
    [SerializeField] private GameObject highlighterOverlay;
    [SerializeField] private GameObject claimedOverlay;

    private void Awake()
    {
        highlighterOverlay.SetActive(false);
        claimedOverlay.SetActive(false);
    }

    public void SetState(State state)
    {
        switch (state)
        {
            case State.HIGHLIGHT:
                highlighterOverlay.SetActive(true);
                claimedOverlay.SetActive(false);
                break;

            case State.CLAIMED:
                highlighterOverlay.SetActive(false);
                claimedOverlay.SetActive(true);
                break;

            default:
                highlighterOverlay.SetActive(false);
                claimedOverlay.SetActive(false);
                break;
        }
    }

    public void SetData(DailyReward rwd, int day, State state = State.NONE)
    {
        dayLabel.text = $"Day {day}";

        iconSlotWidget.SetData(rwd);
        SetState(state);
    }
}