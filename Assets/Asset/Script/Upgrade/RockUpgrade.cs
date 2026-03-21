using UnityEngine;

public class RockUpgradeSystem : MonoBehaviour
{
    public static RockUpgradeSystem Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // persists across scenes
        }
        else
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

    public int GetStableLevel() => PlayerPrefs.GetInt(STABLE_KEY, 0);
    public int GetRecoveryLevel() => PlayerPrefs.GetInt(RECOVERY_KEY, 0);
    public int GetSpeedLevel() => PlayerPrefs.GetInt(SPEED_KEY, 0);
    public int upgradeCost = 5;

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
            Debug.Log("Add ST");
            PlayerPrefs.SetInt(RECOVERY_KEY, level + 1);
            PlayerPrefs.Save();
        }
    }

    public void UpgradeSpeed()
    {
        int level = GetSpeedLevel();
        if (level < maxLevel)
        {
            Debug.Log("Add SP");
            PlayerPrefs.SetInt(SPEED_KEY, level + 1);
            PlayerPrefs.Save();
        }
    }
}