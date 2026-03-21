using UnityEngine;
using System;
using System.Collections.Generic;

public class TokenManager : MonoBehaviour
{
    public static TokenManager Instance;

    private Dictionary<string, int> tokensPerScene = new Dictionary<string, int>();

    public event Action<int> OnTotalTokensUpdated; // total tokens

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
        string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (!tokensPerScene.ContainsKey(scene))
            tokensPerScene[scene] = 0;

        tokensPerScene[scene] += 1;

        OnTotalTokensUpdated?.Invoke(TotalTokens);
        Debug.Log($"Awarded 1 token in {scene}. Total: {TotalTokens}");
    }

    public int TotalTokens
    {
        get
        {
            int total = 0;
            foreach (var t in tokensPerScene.Values)
                total += t;
            return total;
        }
    }

    public bool SpendTokens(int amount)
    {
        if (TotalTokens < 1) return false;

        int remaining = amount;
        List<string> keys = new List<string>(tokensPerScene.Keys);
        foreach (var scene in keys)
        {
            if (remaining <= 0) break;
            int available = tokensPerScene[scene];
            int remove = Mathf.Min(available, remaining);
            tokensPerScene[scene] -= remove;
            remaining -= remove;
        }

        OnTotalTokensUpdated?.Invoke(TotalTokens);
        return true;
    }
}