using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class LevelManager : MonoBehaviour
{
    //function buat restart button
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log(" R Key Pressed! Restarting...");
            restartCurrentLevel();
        }
    }
    public void restartCurrentLevel()
    {
        int currentRestarts = PlayerPrefs.GetInt("RestartCount", 0);

        // ini buat save
        currentRestarts++;

        PlayerPrefs.SetInt("RestartCount", currentRestarts);
        PlayerPrefs.Save();

        Debug.Log("Saved! Total Level Restarts: " + currentRestarts);
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}

