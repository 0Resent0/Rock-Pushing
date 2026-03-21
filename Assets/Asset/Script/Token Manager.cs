using UnityEngine;
using System.Collections.Generic;

public class TokenManager : MonoBehaviour
{
    public static TokenManager Instance;

    [Header("Scene Token Settings")]
    public List<SceneTokenConfig> sceneTokenConfigs = new List<SceneTokenConfig>();

    private Dictionary<string, int> tokensPerScene = new Dictionary<string, int>();

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

        Debug.Log($"Awarded {tokensToAward} token(s) in scene '{currentScene}'. Total: {tokensPerScene[currentScene]}");
    }

    public int GetTokensInScene(string sceneName)
    {
        tokensPerScene.TryGetValue(sceneName, out int count);
        return count;
    }

    public void ResetTokens()
    {
        tokensPerScene.Clear();
    }
}