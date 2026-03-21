using UnityEngine;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public TextMeshProUGUI stableText;
    public TextMeshProUGUI recoveryText;


    void Update()
    {
        stableText.text = "Stable: " + RockUpgradeSystem.Instance.GetStableLevel();
        recoveryText.text = "Recovery: " + RockUpgradeSystem.Instance.GetRecoveryLevel();
    }

    public void UpgradeStable() => RockUpgradeSystem.Instance.UpgradeStable();
    public void UpgradeRecovery() => RockUpgradeSystem.Instance.UpgradeRecovery();
}