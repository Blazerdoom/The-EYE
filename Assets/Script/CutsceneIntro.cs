using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// Plays an intro over the scene: fade in -> show each frame -> fade out,
/// then reveals the gameplay underneath. Builds its own full-screen UI, so
/// just drop it on an empty object and assign the frames.
public class CutsceneIntro : MonoBehaviour
{
    [Header("Cutscene frames, in order")]
    public Sprite[] frames;

    [Header("Timing (seconds)")]
    public float fadeTime = 1f;
    public float holdTime = 2.5f;

    [Header("Play automatically when the scene loads? (Act 2 = yes, Act 1 bed = no)")]
    public bool playOnStart = true;

    [Header("Optional: object to enable when the cutscene finishes (e.g. the EYE ui)")]
    public GameObject revealOnFinish;

    Image _pic;
    Image _black;
    GameObject _root;

    void Start()
    {
        if (playOnStart)
            PlayCutscene();
    }

    /// <summary>Start the cutscene now (e.g. called by the bed when the player sleeps).</summary>
    public void PlayCutscene()
    {
        if (_root != null) return;                       // already playing
        if (frames == null || frames.Length == 0) return;
        BuildUI();
        StartCoroutine(Play());
    }

    void BuildUI()
    {
        _root = new GameObject("CutsceneCanvas");
        var canvas = _root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;
        _root.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        _root.AddComponent<GraphicRaycaster>();

        _black = NewFullImage("Black");
        _black.color = Color.black;                 // starts fully black

        _pic = NewFullImage("Pic");
        _pic.preserveAspect = true;
        _pic.color = new Color(1f, 1f, 1f, 0f);     // starts invisible
    }

    Image NewFullImage(string name)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(_root.transform, false);
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
        return go.AddComponent<Image>();
    }

    IEnumerator Play()
    {
        Time.timeScale = 0f;                             // freeze gameplay during the cutscene
        foreach (var frame in frames)
        {
            _pic.sprite = frame;
            yield return Fade(_pic, 0f, 1f, fadeTime);   // fade the image in
            yield return new WaitForSecondsRealtime(holdTime);
            yield return Fade(_pic, 1f, 0f, fadeTime);   // fade back to black
        }

        yield return Fade(_black, 1f, 0f, fadeTime);     // fade black away -> gameplay
        Time.timeScale = 1f;                             // resume gameplay
        if (revealOnFinish != null)
            revealOnFinish.SetActive(true);              // show the EYE ui, etc.
        Destroy(_root);
    }

    // Safety: never leave the game frozen if this object is destroyed early.
    void OnDestroy()
    {
        if (Time.timeScale == 0f)
            Time.timeScale = 1f;
    }

    static IEnumerator Fade(Image img, float from, float to, float time)
    {
        float e = 0f;
        while (e < time)
        {
            e += Time.unscaledDeltaTime;                 // works while gameplay is frozen
            var c = img.color;
            c.a = Mathf.Lerp(from, to, e / time);
            img.color = c;
            yield return null;
        }
        var fc = img.color; fc.a = to; img.color = fc;
    }
}
