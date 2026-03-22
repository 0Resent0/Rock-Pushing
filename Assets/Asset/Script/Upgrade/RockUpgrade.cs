using UnityEngine;

public class RockUpgradeSystem : MonoBehaviour
{
    private static RockUpgradeSystem _instance;
    public static RockUpgradeSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find existing instance in the scene
                _instance = FindObjectOfType<RockUpgradeSystem>();

                // If none found, create a new one
                if (_instance == null)
                {
                    GameObject obj = new GameObject("RockUpgradeSystem");
                    _instance = obj.AddComponent<RockUpgradeSystem>();
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Keys for PlayerPrefs
    private const string STABLE_KEY = "Upgrade_Stable";
    private const string RECOVERY_KEY = "Upgrade_Recovery";
    private const string SPEED_KEY = "Upgrade_Speed";

    // Upgrade Levels
    public int maxLevel = 10;
    public int upgradeCost = 5;

    public int GetStableLevel() => PlayerPrefs.GetInt(STABLE_KEY, 0);
    public int GetRecoveryLevel() => PlayerPrefs.GetInt(RECOVERY_KEY, 0);
    public int GetSpeedLevel() => PlayerPrefs.GetInt(SPEED_KEY, 0);

    public void UpgradeStable()
    {
        int level = GetStableLevel();
        if (level < maxLevel && GameManager.Instance.SpendTokens(upgradeCost))
        {
            PlayerPrefs.SetInt(STABLE_KEY, level + 1);
            PlayerPrefs.Save();
        }
    }

    public void UpgradeRecovery()
    {
        int level = GetRecoveryLevel();
        if (level < maxLevel)
        {
            PlayerPrefs.SetInt(RECOVERY_KEY, level + 1);
            PlayerPrefs.Save();
        }
    }

    public void UpgradeSpeed()
    {
        int level = GetSpeedLevel();
        if (level < maxLevel)
        {
            PlayerPrefs.SetInt(SPEED_KEY, level + 1);
            PlayerPrefs.Save();
        }
    }
}