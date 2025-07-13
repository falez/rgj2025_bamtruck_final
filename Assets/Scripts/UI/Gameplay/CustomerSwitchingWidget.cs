using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CustomerSwitchingWidget : MonoBehaviour
{
    [SerializeField] private RectTransform start;
    [SerializeField] private RectTransform middle;
    [SerializeField] private RectTransform end;

    [SerializeField] private List<RectTransform> customers;
    private List<int> customerIndex = new();
    private int curCustomerIndex;

    [SerializeField] private float duration = 0.25f;

    private RectTransform currentCustomer;
    private RectTransform nextCustomer;

    private void Awake()
    {
        int i = 0;
        foreach (var c in customers)
        {
            customerIndex.Add(i);
            i++;
            c.anchoredPosition = start.anchoredPosition;
        }

        ResetStates();
    }

    public void ResetStates()
    {
        foreach (var c in customers)
        {
            c.anchoredPosition = start.anchoredPosition;
        }

        currentCustomer = null;
        nextCustomer = null;
    }

    public void Shake(float duration)
    {
        DOTween.Kill(this, true);
        var customer = currentCustomer.GetComponentInChildren<CustomerWidget>();
        customer.MakeAngryFace();
        currentCustomer.DOPunchAnchorPos(new(10.0f, 0.0f), duration, 20).SetId(this);
    }

    public void Boing(float duration, Action onComplete = null)
    {
        DOTween.Kill(this, true);
        var customer = currentCustomer.GetComponentInChildren<CustomerWidget>();
        customer.MakeHappyFace();

        var tr = customer.GetComponent<RectTransform>();
        var tw = tr.DOPunchScale(new(0.0f, 0.1f), duration).SetId(this);

        tw.OnComplete(() =>
        {
            customer.MakeNeutralFace();
            onComplete?.Invoke();
        });
    }

    public void Next(System.Action onComplete)
    {
        Sequence seq = DOTween.Sequence();
        if (currentCustomer == null)
        {
            customerIndex.Shuffle();
            curCustomerIndex = customerIndex[^1];
            customerIndex.RemoveAt(customerIndex.Count - 1);

            nextCustomer = customers[curCustomerIndex];

            var customer = nextCustomer.GetComponentInChildren<CustomerWidget>();
            customer.MakeNeutralFace();

            seq.Append(TweenIn(nextCustomer));
            seq.AppendCallback(() =>
            {
                currentCustomer = nextCustomer;
                nextCustomer = null;
            });
        }
        else
        {
            int oldIndex = curCustomerIndex;
            customerIndex.Shuffle();
            curCustomerIndex = customerIndex[^1];
            customerIndex[^1] = oldIndex;

            nextCustomer = customers[curCustomerIndex];

            var customer = nextCustomer.GetComponentInChildren<CustomerWidget>();
            customer.MakeNeutralFace();

            seq.Append(TweenOut(currentCustomer));
            seq.Join(TweenIn(nextCustomer));
            seq.AppendCallback(() =>
            {
                var customer = currentCustomer.GetComponentInChildren<CustomerWidget>();
                var tr = customer.GetComponent<RectTransform>();
                tr.localScale = Vector3.one;

                currentCustomer = nextCustomer;
                nextCustomer = null;
            });
        }

        seq.OnComplete(() =>
        {
            onComplete?.Invoke();
        });

        seq.Play();
    }

    private Tween TweenIn(RectTransform tr)
    {
        tr.anchoredPosition = start.anchoredPosition;
        return tr.DOAnchorPosX(middle.anchoredPosition.x, duration).SetEase(Ease.Linear);
    }

    private Tween TweenOut(RectTransform tr)
    {
        tr.anchoredPosition = middle.anchoredPosition;
        return tr.DOAnchorPosX(end.anchoredPosition.x, duration).SetEase(Ease.Linear);
    }
}