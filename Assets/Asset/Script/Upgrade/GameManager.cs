using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private const string TOKEN_KEY = "TOKENS";
    private int tokens;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Load(); // ✅ ADD THIS
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Load()
    {
        tokens = PlayerPrefs.GetInt(TOKEN_KEY, 0);
    }

    void Save()
    {
        PlayerPrefs.SetInt(TOKEN_KEY, tokens);
        PlayerPrefs.Save();
    }

    public void AddTokens(int amount)
    {
        tokens += amount;
        Debug.Log("Tokens: " + tokens);
        Save(); // ✅ better to reuse
    }

    public int GetTokens()
    {
        return tokens;
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