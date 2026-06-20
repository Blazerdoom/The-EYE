using UnityEngine;
using System.Collections;

/// <summary>
/// Freezes the game for a few milliseconds on impact. This is the single
/// cheapest trick to make hits feel "heavy". Put this on any persistent
/// object (an empty GameObject called "Juice" is fine), one per scene.
///
/// Call HitStop.Instance.Stop(0.05f) on death, entity grab, big puzzle solve, etc.
/// </summary>
public class HitStop : MonoBehaviour
{
    public static HitStop Instance;

    private bool _isStopped = false;

    void Awake()
    {
        Instance = this;
    }

    /// <summary>Freeze for `duration` seconds of REAL time, then resume.</summary>
    public void Stop(float duration)
    {
        if (_isStopped) return; // don't stack freezes
        StartCoroutine(DoStop(duration));
    }

    private IEnumerator DoStop(float duration)
    {
        _isStopped = true;
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        // WaitForSecondsRealtime ignores timeScale, so it actually waits while frozen.
        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = originalTimeScale;
        _isStopped = false;
    }
}
