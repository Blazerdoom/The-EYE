using UnityEngine;

/// <summary>
/// Put this on a GameObject with a 2D collider set to "Is Trigger".
/// When the player enters, it activates the KeyMinigame AND can reveal the
/// key objects (which start hidden).
///
/// SETUP:
///  1. Box Collider 2D with "Is Trigger" checked.
///  2. Drag your KeyMinigame object into "keyMinigame".
///  3. Drag the key objects (or their parent) into "showOnTrigger" so they appear here.
///  4. Set those objects DISABLED in the scene so they're hidden until the trigger.
///  5. KeyMinigame's "activeOnStart" must be OFF.
/// </summary>
public class KeyMinigameTrigger : MonoBehaviour
{
    [Header("Drag your KeyMinigame object here")]
    public KeyMinigame keyMinigame;

    [Header("Objects to REVEAL when player enters (e.g. the keys). Leave them DISABLED in scene.")]
    public GameObject[] showOnTrigger;

    [Header("Tag the trigger looks for (your player's tag)")]
    public string playerTag = "Player";

    private bool _triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggered) return;
        if (playerTag != "" && !other.CompareTag(playerTag)) return;

        _triggered = true;

        // Reveal the keys / minigame visuals.
        foreach (GameObject go in showOnTrigger)
            if (go != null) go.SetActive(true);

        // Turn on the minigame input.
        if (keyMinigame != null)
            keyMinigame.Activate();
    }
}
