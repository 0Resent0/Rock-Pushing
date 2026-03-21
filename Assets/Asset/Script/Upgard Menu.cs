using UnityEngine;

public class RockUpgradeManager : MonoBehaviour
{
    public static RockUpgradeManager Instance;

    [Header("Levels")]
    public int stabilizeLevel = 0;
    public int recoveryLevel = 0;

    [Header("Base Cost")]
    public int stabilizeBaseCost = 5;
    public int recoveryBaseCost = 5;

    [Header("Scaling")]
    public float costMultiplier = 1.5f;

    [Header("Upgrade Effects")]
    public float stabilizeAmount = 0.05f;
    public float recoveryAmount = 0.2f;

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

    public int GetStabilizeCost() => Mathf.RoundToInt(stabilizeBaseCost * Mathf.Pow(costMultiplier, stabilizeLevel));
    public int GetRecoveryCost() => Mathf.RoundToInt(recoveryBaseCost * Mathf.Pow(costMultiplier, recoveryLevel));

    // Upgrade functions that immediately apply to the rock
    public void UpgradeStabilize(RockController rock)
    {
        if (TokenManager.Instance == null || rock == null) return;
        if (TokenManager.Instance.TotalTokens < 1)
        {
            Debug.Log("Not enough tokens to upgrade Stabilize!");
            return;
        }

        int cost = GetStabilizeCost();
        int spend = Mathf.Min(cost, TokenManager.Instance.TotalTokens);
        TokenManager.Instance.SpendTokens(spend);

        stabilizeLevel++;
        Debug.Log($"Stabilize upgraded to Lv.{stabilizeLevel} (Spent {spend} token(s))");

        ApplyUpgrades(rock);
    }

    public void UpgradeRecovery(RockController rock)
    {
        if (TokenManager.Instance == null || rock == null) return;
        if (TokenManager.Instance.TotalTokens < 1)
        {
            Debug.Log("Not enough tokens to upgrade Recovery!");
            return;
        }

        int cost = GetRecoveryCost();
        int spend = Mathf.Min(cost, TokenManager.Instance.TotalTokens);
        TokenManager.Instance.SpendTokens(spend);

        recoveryLevel++;
        Debug.Log($"Recovery upgraded to Lv.{recoveryLevel} (Spent {spend} token(s))");

        ApplyUpgrades(rock);
    }

    public void ApplyUpgrades(RockController rock)
    {
        if (rock == null) return;

        // reset to base before applying upgrades
        rock.ResetToBase();

        rock.spikeChance = Mathf.Max(0f, rock.spikeChance - stabilizeAmount * stabilizeLevel);
        rock.randomForce = Mathf.Max(0f, rock.randomForce - stabilizeAmount * 20f * stabilizeLevel);
        rock.rockRestore += recoveryAmount * recoveryLevel;
    }

    public float GetStabilizePercent() => stabilizeLevel * stabilizeAmount * 100f;
    public float GetRecoveryPercent() => recoveryLevel * recoveryAmount * 100f;
}