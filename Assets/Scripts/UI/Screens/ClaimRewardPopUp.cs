using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ClaimRewardPopUp : PopUpBase
{
    [SerializeField] private RectTransform content;

    private List<IconSlotWidget> slots = new();

    protected override void Awake()
    {
        base.Awake();

        content.GetComponentsInChildren(slots);
    }

    public void SetData(List<IItemReward> rewards)
    {
        int count = Mathf.Min(slots.Count, rewards.Count);

        foreach (var slot in slots)
        {
            slot.gameObject.SetActive(false);
        }

        for (int i = 0; i < count; i++)
        {
            slots[i].SetData(rewards[i]);
            slots[i].gameObject.SetActive(true);
        }
    }

    protected override void OnShowTransitionCompleted()
    {
        CanvasGroup.interactable = false;
        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(0.5f);
        seq.OnComplete(() =>
        {
            CanvasGroup.interactable = true;
        });
        seq.Play();
    }
}