using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Geometry-Dash-style finish line. Put this on a GameObject with a 2D collider
/// set to "Is Trigger", placed at the END of the chase. When the player reaches
/// it, the level "completes" and loops back to the start.
///
/// SETUP:
///  1. Create an empty GameObject at the end of the level.
///  2. Add a Box Collider 2D with "Is Trigger" checked (make it tall so the
///     player can't run under/over it).
///  3. Put this script on it.
///  4. (Optional) Drag a "Level Complete!" UI panel into completePanel and leave
///     that panel DISABLED in the scene - it flashes for holdTime, then loops.
///  5. nextScene: leave EMPTY to replay THIS level (endless loop, like GD), or
///     type the bedroom hub scene name (e.g. "Act 1") to return to level select.
/// </summary>
public class FinishLine : MonoBehaviour
{
    [Header("Tag the finish looks for (your player's tag)")]
    public string playerTag = "Player";

    [Header("Optional: 'Level Complete' panel to flash (leave DISABLED in scene)")]
    public GameObject completePanel;

    [Header("Seconds to show the complete panel before looping")]
    public float holdTime = 1.5f;

    [Header("Scene to load next. EMPTY = replay this level (loop).")]
    public string nextScene = "";

    private bool _done = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_done) return;
        if (!other.CompareTag(playerTag)) return;

        _done = true;

        if (completePanel != null)
        {
            completePanel.SetActive(true);
            Invoke(nameof(GoNext), holdTime);
        }
        else
        {
            Invoke(nameof(GoNext), 0.2f);
        }
    }

    private void GoNext()
    {
        Time.timeScale = 1f;   // safety: never load while the game is paused/frozen

        string scene = string.IsNullOrEmpty(nextScene)
            ? SceneManager.GetActiveScene().name   // reload this level = loop
            : nextScene;

        if (SceneFader.Instance != null)
            SceneFader.Instance.FadeToScene(scene);
        else
            SceneManager.LoadScene(scene);
    }
}
