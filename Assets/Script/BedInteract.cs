using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class BedInteract : MonoBehaviour
{
    public GameObject eButton;       // the "press E" prompt
    public Image fadeImage;          // the black FadePanel image (used only if there's no SceneFader)
    public string chaseSceneName = "Act 2";   // the chase scene the bed loads into

    bool isInArea = false;
    bool used = false;

    void Update()
    {
        if (isInArea && !used && Input.GetKeyDown(KeyCode.E))
        {
            used = true;
            if (eButton != null) eButton.SetActive(false);
            StartCoroutine(Sleep());
        }
    }

    IEnumerator Sleep()
    {
        // If a nightmare cutscene exists, play it and let it load the chase scene
        // when it finishes (screen stays black, so the transition is seamless).
        var cutscene = FindObjectOfType<CutsceneIntro>();
        if (cutscene != null)
        {
            cutscene.loadSceneOnFinish = chaseSceneName;
            cutscene.PlayCutscene();
            yield break;
        }

        // Fallback (no cutscene object): fade to black, then load the chase scene.
        if (SceneFader.Instance != null)
        {
            SceneFader.Instance.FadeToScene(chaseSceneName);
        }
        else
        {
            if (fadeImage != null)
            {
                fadeImage.gameObject.SetActive(true);
                fadeImage.DOFade(1f, 0.6f);   // fade to black
                yield return new WaitForSeconds(0.7f);
            }
            SceneManager.LoadScene(chaseSceneName);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInArea = true;
            if (eButton != null) eButton.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInArea = false;
            if (eButton != null) eButton.SetActive(false);
        }
    }
}