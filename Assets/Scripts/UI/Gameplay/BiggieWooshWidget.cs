using DG.Tweening;
using UnityEngine;

public class BiggieWooshWidget : MonoBehaviour
{
    [SerializeField] private RectTransform start;
    [SerializeField] private RectTransform end;

    [SerializeField] private RectTransform woosher;
    [SerializeField] private float duration = 0.25f;
    [SerializeField] private Ease easing = Ease.Linear;

    public void Play(System.Action onReachMiddle = null)
    {
        DOTween.Kill(this, false);
        woosher.anchoredPosition = start.anchoredPosition;

        Sequence seq = DOTween.Sequence(this);

        seq.Append(
        woosher.DOAnchorPosX(end.anchoredPosition.x, duration)
            .SetEase(easing)
            .OnUpdate(() =>
        {
            if (woosher.anchoredPosition.x < 0)
                onReachMiddle?.Invoke();
        }));
        seq.Append(woosher.DOAnchorPosX(end.anchoredPosition.x * 2.0f, duration * 0.5f));

        seq.Play();
    }
}