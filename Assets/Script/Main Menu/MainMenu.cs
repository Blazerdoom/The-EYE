using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

/// <summary>
/// Controls the main menu buttons.
///
/// CREDITS: the old QUIT button now opens a Credits screen. If you leave
/// "creditsPanel" empty, the script BUILDS a tidy credits overlay at runtime
/// using the sponsor logos + text you set below - no UI to build by hand.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [Header("Scene to load when PLAY is pressed")]
    public string firstSceneName = "Act 1";

    [Header("Drag your Settings popup panel here (can be left empty)")]
    public GameObject settingsPanel;

    [Header("Optional: your own Credits panel. Leave empty to auto-build one.")]
    public GameObject creditsPanel;

    [Header("CREDITS CONTENT (used when no Credits panel is set)")]
    [Tooltip("Drag your sponsor logo images here")]
    public Sprite[] sponsorLogos;
    [TextArea(3, 12)]
    public string creditsText =
        "MUSIC\n" +
        "Nightmare - spmusic\n" +
        "Lucid Dream - narzeky\n\n" +
        "SFX\n" +
        "Correct / Incorrect - LaurenPounder\n" +
        "Heartbeat - loudernoises\n" +
        "Footstep - EvanBoyerman\n\n" +
        "TEAM\n" +
        "Ben Surya - Coder\n" +
        "Jen - 2D Artist";

    GameObject _runtimeCredits;

    void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    /// <summary>Hook this to the PLAY button.</summary>
    public void PlayGame()
    {
        if (SceneFader.Instance != null)
            SceneFader.Instance.FadeToScene(firstSceneName);
        else
            SceneManager.LoadScene(firstSceneName);
    }

    /// <summary>Hook this to the SETTINGS button.</summary>
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
            settingsPanel.transform.localScale = Vector3.one * 0.8f;
            settingsPanel.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
        }
    }

    /// <summary>Hook this to the Settings panel's CLOSE/back button.</summary>
    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    /// <summary>Hook this to the CREDITS button (the former Quit button).</summary>
    public void OpenCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
            creditsPanel.transform.localScale = Vector3.one * 0.8f;
            creditsPanel.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
            return;
        }

        if (_runtimeCredits != null)
            Destroy(_runtimeCredits);
        _runtimeCredits = BuildCreditsOverlay();
        _runtimeCredits.transform.localScale = Vector3.one * 0.85f;
        _runtimeCredits.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
    }

    /// <summary>Hook this to the Credits panel's CLOSE/back button.</summary>
    public void CloseCredits()
    {
        if (_runtimeCredits != null)
        {
            Destroy(_runtimeCredits);
            _runtimeCredits = null;
        }
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }

    /// <summary>Hook this to the QUIT button if you still want one.</summary>
    public void QuitGame()
    {
        Debug.Log("Quit pressed");
        Application.Quit();
    }

    // ---- runtime credits overlay: one tidy centered column ----

    GameObject BuildCreditsOverlay()
    {
        var root = new GameObject("RuntimeCredits");
        var canvas = root.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        root.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        root.AddComponent<GraphicRaycaster>();

        // Dim full-screen background.
        var bg = NewChild("BG", root.transform);
        Stretch(bg);
        bg.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.92f);

        // One centered column that lays its children out top-to-bottom.
        var col = NewChild("Column", root.transform);
        var crt = col.GetComponent<RectTransform>();
        crt.anchorMin = crt.anchorMax = crt.pivot = new Vector2(0.5f, 0.5f);
        crt.sizeDelta = new Vector2(820f, 0f);
        var vlg = col.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = 16;
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.childControlWidth = true; vlg.childControlHeight = true;
        vlg.childForceExpandWidth = true; vlg.childForceExpandHeight = false;
        col.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // Title.
        AddLabel(col.transform, "CREDITS", 56, FontStyle.Bold);

        // Middle row: credits list on the LEFT, sponsor logo on the RIGHT.
        var row = NewChild("Row", col.transform);
        var rowLayout = row.AddComponent<HorizontalLayoutGroup>();
        rowLayout.spacing = 40;
        rowLayout.childAlignment = TextAnchor.MiddleCenter;
        rowLayout.childControlWidth = true; rowLayout.childControlHeight = true;
        rowLayout.childForceExpandWidth = false; rowLayout.childForceExpandHeight = false;

        // Left: the credits list.
        var listGo = NewChild("List", row.transform);
        listGo.AddComponent<LayoutElement>().preferredWidth = 430;
        AddText(listGo, creditsText, 26, TextAnchor.UpperLeft, FontStyle.Normal);

        // Right: sponsor label + logo(s). Use the Inspector field, or fall back
        // to the Resources/CreditLogos folder so logos show even if nothing is wired.
        var logos = (sponsorLogos != null && sponsorLogos.Length > 0)
            ? sponsorLogos
            : Resources.LoadAll<Sprite>("CreditLogos");
        if (logos != null && logos.Length > 0)
        {
            var sponsorCol = NewChild("Sponsor", row.transform);
            var scLayout = sponsorCol.AddComponent<VerticalLayoutGroup>();
            scLayout.spacing = 10;
            scLayout.childAlignment = TextAnchor.MiddleCenter;
            scLayout.childControlWidth = true; scLayout.childControlHeight = true;
            scLayout.childForceExpandWidth = false; scLayout.childForceExpandHeight = false;
            sponsorCol.AddComponent<LayoutElement>().preferredWidth = 300;

            AddLabel(sponsorCol.transform, "SPONSOR", 30, FontStyle.Bold);
            foreach (var logo in logos)
            {
                if (logo == null) continue;
                var img = NewChild("Logo", sponsorCol.transform);
                var le = img.AddComponent<LayoutElement>();
                le.preferredHeight = 170; le.preferredWidth = 290;
                var image = img.AddComponent<Image>();
                image.sprite = logo;
                image.preserveAspect = true;
            }
        }

        // Back button.
        var btn = NewChild("BackButton", col.transform);
        var ble = btn.AddComponent<LayoutElement>();
        ble.preferredHeight = 64; ble.preferredWidth = 220; ble.flexibleWidth = 0;
        btn.AddComponent<Image>().color = new Color(1f, 1f, 1f, 0.18f);
        btn.AddComponent<Button>().onClick.AddListener(CloseCredits);
        var bt = NewChild("Text", btn.transform);
        Stretch(bt);
        AddText(bt, "BACK", 28, TextAnchor.MiddleCenter, FontStyle.Bold);

        return root;
    }

    static void AddLabel(Transform parent, string content, int size, FontStyle style)
    {
        var go = NewChild("Label", parent);
        AddText(go, content, size, TextAnchor.UpperCenter, style);
    }

    static GameObject NewChild(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.transform.SetParent(parent, false);
        return go;
    }

    static void Stretch(GameObject go)
    {
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;
    }

    static void AddText(GameObject go, string content, int size, TextAnchor anchor, FontStyle style)
    {
        var t = go.AddComponent<Text>();
        t.text = content;
        t.fontSize = size;
        t.fontStyle = style;
        t.alignment = anchor;
        t.color = Color.white;
        t.lineSpacing = 1.1f;
        t.horizontalOverflow = HorizontalWrapMode.Wrap;
        t.verticalOverflow = VerticalWrapMode.Overflow;
        Font f = null;
        try { f = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); } catch { }
        if (f == null) { try { f = Resources.GetBuiltinResource<Font>("Arial.ttf"); } catch { } }
        t.font = f;
    }
}
