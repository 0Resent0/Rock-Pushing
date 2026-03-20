using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void OnPlayAgainButton()
    {
        // Load the scene that caused the Game Over
        string lastGameplayScene = SceneTracker.PreviousScene;

        if (!string.IsNullOrEmpty(lastGameplayScene))
        {
            SceneManager.LoadScene(lastGameplayScene);
        }
        else
        {
            Debug.LogWarning("Previous scene not found! Loading default scene.");
            SceneManager.LoadScene("MainMenu");
        }
    }
}
