using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int tokens;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Load()
    {
        tokens = PlayerPrefs.GetInt("TOKENS", 0);
    }

    void Save()
    {
        PlayerPrefs.SetInt("TOKENS", tokens);
        PlayerPrefs.Save();
    }

    public int GetTokens()
    {
        return tokens;
    }

    public void AddTokens(int amount)
    {
        tokens += amount;                  // ✅ increment tokens
        Debug.Log("Tokens: " + tokens);
        PlayerPrefs.SetInt("Tokens", tokens); // save persistently
        PlayerPrefs.Save();
    }

    public bool SpendTokens(int amount)
    {
        if (tokens >= amount)
        {
            Debug.Log("Spend");
            tokens -= amount;
            Save();
            return true;
        }
        return false;
    }
}