using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// Memory key minigame (shell-game style):
///  1. Correct key FLASHES GREEN at its slot (player memorizes the slot/key).
///  2. Keys SHUFFLE for ~shuffleDuration seconds: every round, ALL keys are
///     reassigned to the fixed slot positions in a new random order, so they
///     always snap to clean slots (never drift/clutter).
///  3. Shuffle ends -> player presses a number (1-8) for a SLOT.
///  4. If the key now in that slot is the one that was green, GOOD ending; else BAD.
///
/// SETUP:
///  - Put on "KeyMinigame". Drag the 8 key Transforms into "keys".
///  - The keys' CURRENT positions become the fixed slots.
///  - Each key needs a SpriteRenderer (for the green flash).
///  - Drag Good/Bad ending panels (DISABLED in scene).
/// </summary>
public class KeyMinigame : MonoBehaviour
{
    [Header("Drag your key objects here. Their start positions = the fixed slots.")]
    public Transform[] keys;

    [Header("Ending UI panels (leave DISABLED in scene)")]
    public GameObject goodEndingPanel;
    public GameObject badEndingPanel;

    [Header("Hide these when an ending shows (optional)")]
    public GameObject[] hideOnEnding;

    [Header("Shuffle settings")]
    public float shuffleDuration = 8f;   // total shuffle time in seconds
    public float roundTime = 0.4f;       // seconds per shuffle round (all keys move)
    public float greenFlashTime = 1.5f;  // how long the correct key glows first

    [Header("Sounds (optional)")]
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip shuffleSound;

    [Header("Start active without trigger (testing)")]
    public bool activeOnStart = false;

    private int _correctKeyIndex;     // which KEY object is correct
    private Vector3[] _slots;         // fixed slot positions
    private bool _canPick = false;
    private bool _chosen = false;
    private bool _onBadEnding = false;  // true once bad ending shows, enables R to retry
    private SpriteRenderer[] _renderers;
    private Color[] _baseColors;

    void Start()
    {
        if (goodEndingPanel != null) goodEndingPanel.SetActive(false);
        if (badEndingPanel != null) badEndingPanel.SetActive(false);

        _correctKeyIndex = Random.Range(0, keys.Length);
        // NOTE: slot positions are captured in Activate(), once keys are visible.

        if (activeOnStart) Activate();
    }

    public void Activate()
    {
        // Capture slot positions NOW (keys are visible/enabled at this point).
        _slots = new Vector3[keys.Length];
        _renderers = new SpriteRenderer[keys.Length];
        _baseColors = new Color[keys.Length];
        for (int i = 0; i < keys.Length; i++)
        {
            _slots[i] = keys[i].localPosition;
            _renderers[i] = keys[i].GetComponent<SpriteRenderer>();
            if (_renderers[i] != null) _baseColors[i] = _renderers[i].color;
        }
        StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        // 1. Flash correct key green.
        SpriteRenderer correct = _renderers[_correctKeyIndex];
        if (correct != null)
        {
            correct.DOColor(Color.green, 0.25f);
            keys[_correctKeyIndex].DOPunchScale(keys[_correctKeyIndex].localScale * 0.2f, 0.4f, 6, 0.7f);
        }
        yield return new WaitForSeconds(greenFlashTime);
        if (correct != null) correct.DOColor(_baseColors[_correctKeyIndex], 0.25f);
        yield return new WaitForSeconds(0.25f);

        // 2. Shuffle for shuffleDuration. Each round, ALL keys are reassigned
        //    to the fixed slots in a new random order, then tween there together.
        float elapsed = 0f;
        while (elapsed < shuffleDuration)
        {
            // Make a shuffled order of slot indices.
            List<int> order = new List<int>();
            for (int i = 0; i < _slots.Length; i++) order.Add(i);
            // Fisher-Yates shuffle.
            for (int i = order.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                int tmp = order[i]; order[i] = order[j]; order[j] = tmp;
            }

            if (shuffleSound != null && GameAudio.Instance != null)
                GameAudio.Instance.PlaySFX(shuffleSound);

            // Move every key to its newly assigned slot at once.
            for (int i = 0; i < keys.Length; i++)
                keys[i].DOLocalMove(_slots[order[i]], roundTime).SetEase(Ease.InOutQuad);

            yield return new WaitForSeconds(roundTime);
            elapsed += roundTime;
        }

        _canPick = true;
    }

    void Update()
    {
        // On the bad ending, press R to retry the whole Act 2 scene.
        if (_onBadEnding && Input.GetKeyDown(KeyCode.R))
        {
            // Make sure time is running normally before reloading.
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        if (!_canPick || _chosen) return;

        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ChooseSlot(i);
            }
        }
    }

    // Player presses a NUMBER for a SLOT. Find which key sits in that slot now.
    private void ChooseSlot(int slotIndex)
    {
        _chosen = true;

        Vector3 slotPos = _slots[slotIndex];
        int closestKey = 0;
        float closestDist = Mathf.Infinity;
        for (int i = 0; i < keys.Length; i++)
        {
            float d = Vector3.Distance(keys[i].localPosition, slotPos);
            if (d < closestDist) { closestDist = d; closestKey = i; }
        }

        keys[closestKey].DOPunchScale(keys[closestKey].localScale * 0.3f, 0.3f, 8, 0.7f);

        bool isCorrect = (closestKey == _correctKeyIndex);

        if (isCorrect)
        {
            if (GameAudio.Instance != null) GameAudio.Instance.PlaySFX(correctSound);
            Invoke(nameof(ShowGood), 0.6f);
        }
        else
        {
            if (GameAudio.Instance != null) GameAudio.Instance.PlaySFX(wrongSound);
            if (CameraShake.Instance != null) CameraShake.Instance.Shake(0.4f, 0.5f);
            Invoke(nameof(ShowBad), 0.6f);
        }
    }

    private void ShowGood()
    {
        HideGameplay();
        if (goodEndingPanel != null) { goodEndingPanel.SetActive(true); PopIn(goodEndingPanel); }
    }

    private void ShowBad()
    {
        HideGameplay();
        _onBadEnding = true;  // allow retry now
        if (badEndingPanel != null) { badEndingPanel.SetActive(true); PopIn(badEndingPanel); }
    }

    private void HideGameplay()
    {
        foreach (GameObject go in hideOnEnding)
            if (go != null) go.SetActive(false);
    }

    private void PopIn(GameObject panel)
    {
        panel.transform.localScale = Vector3.one * 0.8f;
        panel.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }
}
