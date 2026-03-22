using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void OnPlayAgainButton()
    {
        string lastGameplayScene = SceneTracker.PreviousScene;

        if (!string.IsNullOrEmpty(lastGameplayScene))
        {
            SceneTracker.ResetTracker();
            SceneManager.LoadScene(lastGameplayScene);
        }
        else
        {
            Debug.LogWarning("Previous scene not found! Loading default scene.");
            SceneTracker.ResetTracker();
            SceneManager.LoadScene("Ads 1");
        }
    }
}