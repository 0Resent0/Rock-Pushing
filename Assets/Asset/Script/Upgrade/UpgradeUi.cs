using UnityEngine;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public TextMeshProUGUI stableText;
    public TextMeshProUGUI recoveryText;

    void Update()
    {
        UpdateUpgradeText();
    }

    void UpdateUpgradeText()
    {
        var sys = RockUpgradeSystem.Instance;

        UpdateSingle(stableText, sys.GetStableLevel(), sys.maxLevel);
        UpdateSingle(recoveryText, sys.GetRecoveryLevel(), sys.maxLevel);
    }

    void UpdateSingle(TextMeshProUGUI text, int level, int max)
    {
        if (text == null) return;

        float percent = ((float)level / max) * 100f;
        int rounded = Mathf.RoundToInt(percent);

        text.text = level + " (" + rounded + "%)";
    }

    // Button functions
    public void UpgradeStable() => RockUpgradeSystem.Instance.UpgradeStable();
    public void UpgradeRecovery() => RockUpgradeSystem.Instance.UpgradeRecovery();
}