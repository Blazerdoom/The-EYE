using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

/// <summary>
/// Handles fade-to-black scene transitions.
///
/// SETUP (do this once per scene, explained in the chat steps):
///  1. Make a Canvas (Screen Space - Overlay).
///  2. Inside it, make a full-screen black Image called "FadePanel".
///  3. Put this script on the Canvas (or the FadePanel).
///  4. Drag the FadePanel's Image into the "fadeImage" slot in the Inspector.
///
/// USE: instead of SceneManager.LoadScene("Act 2"),
///      call SceneFader.Instance.FadeToScene("Act 2");
/// </summary>
public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance;

    [Header("Drag your full-screen black Image here")]
    public Image fadeImage;

    [Header("Timing")]
    public float fadeDuration = 0.6f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // When a scene opens, start black and fade OUT to reveal the scene.
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            Color c = fadeImage.color;
            c.a = 1f;                 // start fully black
            fadeImage.color = c;
            fadeImage.DOFade(0f, fadeDuration); // fade to transparent
        }
    }

    /// <summary>Fade to black, THEN load the new scene.</summary>
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            // Fade IN to black over fadeDuration.
            fadeImage.DOFade(1f, fadeDuration);
            // Wait for the fade to finish before loading.
            yield return new WaitForSeconds(fadeDuration);
        }

        SceneManager.LoadScene(sceneName);
    }
}
