using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTracker : MonoBehaviour
{
    public static string PreviousScene { get; private set; } = "";
    public static string CurrentScene { get; private set; } = "";

    private bool initialized = false;

    private void Awake()
    {
        // Prevent duplicates
        if (FindObjectsOfType<SceneTracker>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private bool IsGameplayScene(string sceneName)
    {
        return sceneName != "GameOver" && sceneName != "MainMenu";
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!initialized)
        {
            CurrentScene = scene.name;
            initialized = true;
            return;
        }

        // Save the scene we are leaving (if it's gameplay)
        if (IsGameplayScene(CurrentScene))
        {
            PreviousScene = CurrentScene;
        }

        CurrentScene = scene.name;

        Debug.Log($"Current: {CurrentScene} | Previous: {PreviousScene}");
    }

    // ✅ Add this so GameOver can call it
    public static void ResetTracker()
    {
        PreviousScene = "";
        CurrentScene = "";
    }
}