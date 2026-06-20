using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BedInteract : MonoBehaviour
{
    public GameObject eButton;       // the "press E" prompt
    public Transform spawnPoint;     // BedSpawn
    public Transform player;         // the Player object
    public Image fadeImage;          // the black FadePanel image

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
        // If an Act 1 cutscene exists, let it handle fade -> frames -> fade,
        // and move the player into the nightmare while the screen is black.
        var cutscene = FindObjectOfType<CutsceneIntro>();
        if (cutscene != null)
        {
            cutscene.PlayCutscene();
            if (player != null && spawnPoint != null)
                player.position = spawnPoint.position;
            yield break;
        }

        // Fallback: simple fade-teleport.
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.DOFade(1f, 0.6f);   // fade to black
            yield return new WaitForSeconds(0.7f);
        }

        if (player != null && spawnPoint != null)
            player.position = spawnPoint.position;

        if (fadeImage != null)
        {
            fadeImage.DOFade(0f, 0.6f);   // fade back in
        }
        yield return null;
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