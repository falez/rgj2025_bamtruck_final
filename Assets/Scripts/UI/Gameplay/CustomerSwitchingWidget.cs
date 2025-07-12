using System;
using DG.Tweening;
using UnityEngine;

public class CustomerSwitchingWidget : MonoBehaviour
{
    [SerializeField] private RectTransform start;
    [SerializeField] private RectTransform middle;
    [SerializeField] private RectTransform end;

    [SerializeField] private RectTransform customer1;
    [SerializeField] private RectTransform customer2;

    [SerializeField] private float duration = 0.25f;

    private RectTransform currentCustomer;
    private RectTransform nextCustomer;

    private void Awake()
    {
        ResetStates();
    }

    public void ResetStates()
    {
        customer1.anchoredPosition = start.anchoredPosition;
        customer2.anchoredPosition = start.anchoredPosition;
        currentCustomer = null;
        nextCustomer = null;
    }

    public void Shake(float duration)
    {
        var customer = currentCustomer.GetComponentInChildren<CustomerWidget>();
        customer.MakeAngryFace();
        currentCustomer.DOPunchAnchorPos(new(10.0f, 0.0f), duration, 20);
    }

    public void Boing(float duration, Action onComplete = null)
    {
        var customer = currentCustomer.GetComponentInChildren<CustomerWidget>();
        customer.MakeHappyFace();

        var tr = customer.GetComponent<RectTransform>();
        var tw = tr.DOPunchScale(new(0.0f, 0.1f), duration);

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
            (currentCustomer, nextCustomer) = (customer1, customer2);

            var customer = currentCustomer.GetComponentInChildren<CustomerWidget>();
            customer.MakeNeutralFace();
            seq.Append(TweenIn(currentCustomer));
        }
        else
        {
            var customer = nextCustomer.GetComponentInChildren<CustomerWidget>();
            customer.MakeNeutralFace();

            seq.Append(TweenOut(currentCustomer));
            seq.Join(TweenIn(nextCustomer));
            seq.AppendCallback(() =>
            {
                var customer = currentCustomer.GetComponentInChildren<CustomerWidget>();
                var tr = customer.GetComponent<RectTransform>();
                tr.localScale = Vector3.one;

                (nextCustomer, currentCustomer) = (currentCustomer, nextCustomer);
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