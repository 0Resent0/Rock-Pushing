using UnityEngine;
using System;
using System.Collections.Generic;

public class TokenManager : MonoBehaviour
{
    public static TokenManager Instance;

    [Header("Scene Token Settings")]
    public List<SceneTokenConfig> sceneTokenConfigs = new List<SceneTokenConfig>();

    private Dictionary<string, int> tokensPerScene = new Dictionary<string, int>();

    // Event to notify when tokens are updated
    public event Action<int> OnTotalTokensUpdated; // total tokens across all scenes

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AwardToken()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        int tokensToAward = 1;
        SceneTokenConfig config = sceneTokenConfigs.Find(c => c.sceneName == currentScene);
        if (config != null)
            tokensToAward = config.tokensToAward;

        if (!tokensPerScene.ContainsKey(currentScene))
            tokensPerScene[currentScene] = 0;

        tokensPerScene[currentScene] += tokensToAward;

        Debug.Log($"Awarded {tokensToAward} token(s) in scene '{currentScene}'. Total in scene: {tokensPerScene[currentScene]}");

        // Notify listeners of the new total tokens
        OnTotalTokensUpdated?.Invoke(TotalTokens);
    }

    public int TotalTokens
    {
        get
        {
            int total = 0;
            foreach (var count in tokensPerScene.Values)
                total += count;
            return total;
        }
    }

    public int GetTokensInScene(string sceneName)
    {
        tokensPerScene.TryGetValue(sceneName, out int count);
        return count;
    }

    public void ResetTokens()
    {
        tokensPerScene.Clear();
        OnTotalTokensUpdated?.Invoke(0);
    }
}