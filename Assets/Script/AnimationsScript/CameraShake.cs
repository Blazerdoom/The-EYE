using UnityEngine;
using DG.Tweening;

/// <summary>
/// Drop this on your Camera (the one inside "mc" in Act 1, and the Act 2 camera).
/// Call CameraShake.Instance.Shake(...) from anywhere to shake the screen.
/// </summary>
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    [Header("Default shake settings")]
    public float defaultDuration = 0.2f;
    public float defaultStrength = 0.3f;
    public int defaultVibrato = 20;

    private Vector3 _startPos;
    private Tween _currentShake;

    void Awake()
    {
        // Simple singleton so you can call it from any script.
        Instance = this;
        _startPos = transform.localPosition;
    }

    /// <summary>Shake with default values. Good for small hits.</summary>
    public void Shake()
    {
        Shake(defaultDuration, defaultStrength, defaultVibrato);
    }

    /// <summary>Shake with custom values. Use bigger strength for death/big events.</summary>
    public void Shake(float duration, float strength, int vibrato = 20)
    {
        // Kill any shake already running so they don't fight each other.
        if (_currentShake != null && _currentShake.IsActive())
            _currentShake.Kill();

        // Always reset to the resting position before shaking.
        transform.localPosition = _startPos;

        _currentShake = transform.DOShakePosition(duration, strength, vibrato, 90, false, true)
            .OnComplete(() => transform.localPosition = _startPos);
    }
}
