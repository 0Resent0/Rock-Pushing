using UnityEngine;

public class RockUpgradeManager : MonoBehaviour
{
    public static RockUpgradeManager Instance;

    [Header("Upgrade Levels")]
    public int stabilizeLevel = 0;    // each level reduces spikeChance & randomForce
    public int recoveryLevel = 0;     // each level increases rockRestore

    [Header("Upgrade Settings")]
    public float stabilizeAmount = 0.05f;  // per level
    public float recoveryAmount = 0.2f;    // per level

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

    // Apply upgrades to a RockController
    public void ApplyUpgrades(RockController rock)
    {
        if (rock == null) return;

        rock.spikeChance = Mathf.Max(0f, rock.spikeChance - stabilizeAmount * stabilizeLevel);
        rock.randomForce = Mathf.Max(0f, rock.randomForce - stabilizeAmount * 20f * stabilizeLevel);
        rock.rockRestore += recoveryAmount * recoveryLevel;
    }

    // Upgrade functions (call from buttons)
    public void UpgradeStabilize()
    {
        stabilizeLevel++;
        Debug.Log("Stabilize upgraded! Level: " + stabilizeLevel);
    }

    public void UpgradeRecovery()
    {
        recoveryLevel++;
        Debug.Log("Recovery upgraded! Level: " + recoveryLevel);
    }
}