using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTracker : MonoBehaviour
{
    public static string PreviousScene { get; private set; } = "";
    public static string CurrentScene { get; private set; } = "";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update PreviousScene only if CurrentScene is not empty
        if (!string.IsNullOrEmpty(CurrentScene))
            PreviousScene = CurrentScene;

        CurrentScene = scene.name;
        Debug.Log("Scene Loaded: " + scene.name + " | Previous: " + PreviousScene);
    }
}