using UnityEngine;

public class RockSceneInitializer : MonoBehaviour
{
    public RockController rock;

    private void Start()
    {
        if (RockUpgradeManager.Instance != null)
        {
            RockUpgradeManager.Instance.ApplyUpgrades(rock);
        }
    }
}