using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToAct2 : MonoBehaviour
{
    public string sceneName = "Act 2";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (SceneFader.Instance != null)
                SceneFader.Instance.FadeToScene(sceneName);
            else
                SceneManager.LoadScene(sceneName);
        }
    }
}