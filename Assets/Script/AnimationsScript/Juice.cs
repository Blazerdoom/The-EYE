using UnityEngine;
using DG.Tweening;

/// <summary>
/// A bag of reusable juice effects you can call on ANY object.
/// Add this component to anything you want to "pop", or just call the
/// static helpers from other scripts: Juice.Pop(myTransform);
/// </summary>
public class Juice : MonoBehaviour
{
    // ---------- INSTANCE HELPERS (put component on the object) ----------

    private Vector3 _baseScale;

    void Awake()
    {
        _baseScale = transform.localScale;
    }

    /// <summary>Quick scale punch. Great for buttons, pickups, puzzle pieces clicking in.</summary>
    public void Pop(float strength = 0.25f, float duration = 0.3f)
    {
        transform.DOKill();
        transform.localScale = _baseScale;
        transform.DOPunchScale(_baseScale * strength, duration, 8, 0.7f);
    }

    // ---------- STATIC HELPERS (call from anywhere, no component needed) ----------

    /// <summary>Punch-scale any transform. e.g. Juice.Pop(transform);</summary>
    public static void Pop(Transform t, float strength = 0.25f, float duration = 0.3f)
    {
        t.DOKill();
        Vector3 baseScale = t.localScale;
        t.DOPunchScale(baseScale * strength, duration, 8, 0.7f)
            .OnComplete(() => t.localScale = baseScale);
    }

    /// <summary>Squash & stretch toward a target scale then settle back. Good for jumps/landings.</summary>
    public static void Squash(Transform t, Vector3 squashScale, float duration = 0.15f)
    {
        Vector3 baseScale = t.localScale;
        t.DOKill();
        Sequence s = DOTween.Sequence();
        s.Append(t.DOScale(squashScale, duration).SetEase(Ease.OutQuad));
        s.Append(t.DOScale(baseScale, duration).SetEase(Ease.OutBack));
    }

    /// <summary>Fade a SpriteRenderer or UI element in. Pass the CanvasGroup or SpriteRenderer's color setter.</summary>
    public static void FadeInSprite(SpriteRenderer sr, float duration = 0.4f)
    {
        Color c = sr.color;
        c.a = 0f;
        sr.color = c;
        sr.DOFade(1f, duration);
    }
}
