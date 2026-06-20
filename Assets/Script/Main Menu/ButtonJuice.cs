using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// Makes a UI button feel alive: grows slightly on hover, pops on click.
/// Put this component on EACH button you want juiced (PLAY, SETTINGS, QUIT).
/// No setup needed beyond adding it.
/// </summary>
public class ButtonJuice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [Header("How big the button grows on hover (1.1 = 10% bigger)")]
    public float hoverScale = 1.1f;
    public float tweenTime = 0.15f;

    private Vector3 _baseScale;

    void Awake()
    {
        _baseScale = transform.localScale;
    }

    // Mouse enters the button.
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(_baseScale * hoverScale, tweenTime).SetEase(Ease.OutQuad);
    }

    // Mouse leaves the button.
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(_baseScale, tweenTime).SetEase(Ease.OutQuad);
    }

    // Button is clicked.
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOKill();
        transform.localScale = _baseScale;
        transform.DOPunchScale(_baseScale * 0.2f, 0.3f, 8, 0.7f);
    }
}
