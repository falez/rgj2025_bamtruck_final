using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(CanvasGroup))]
public class GameStartWidget : MonoBehaviour
{
    private Animation anim;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        anim = GetComponent<Animation>();
        canvasGroup.alpha = 0.0f;
    }

    public void Play(System.Action onComplete = null)
    {
        canvasGroup.alpha = 1.0f;
        anim.Play();

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1.25f);

        seq.AppendCallback(() =>
        {
            canvasGroup.alpha = 0.0f;
        });

        seq.AppendInterval(0.25f);
        seq.OnComplete(() =>
        {
            onComplete?.Invoke();
        });
        seq.Play();
    }

    public void AnimationEnd()
    {
    }
}