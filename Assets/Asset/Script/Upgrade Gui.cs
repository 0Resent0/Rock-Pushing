using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public TMP_Text stabilizeLevelText;
    public Button stabilizeButton;
    public TMP_Text recoveryLevelText;
    public Button recoveryButton;

    public RockController playerRock; // assign in inspector

    private RockUpgradeManager upgrade;
    private TokenManager token;

    void Start()
    {
        upgrade = RockUpgradeManager.Instance;
        token = TokenManager.Instance;

        UpdateUI();

        if (token != null)
            token.OnTotalTokensUpdated += OnTokenChanged;
    }

    private void OnDestroy()
    {
        if (token != null)
            token.OnTotalTokensUpdated -= OnTokenChanged;
    }

    void OnTokenChanged(int total)
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (upgrade == null || token == null) return;

        stabilizeLevelText.text = $"Lv.{upgrade.stabilizeLevel} (+{upgrade.GetStabilizePercent():0}%)";
        recoveryLevelText.text = $"Lv.{upgrade.recoveryLevel} (+{upgrade.GetRecoveryPercent():0}%)";

        // buttons active only if player has at least 1 token
        stabilizeButton.interactable = token.TotalTokens >= 1;
        recoveryButton.interactable = token.TotalTokens >= 1;
    }

    public void OnClickUpgradeStabilize()
    {
        if (upgrade == null || playerRock == null) return;
        upgrade.UpgradeStabilize(playerRock);
        UpdateUI();
    }

    public void OnClickUpgradeRecovery()
    {
        if (upgrade == null || playerRock == null) return;
        upgrade.UpgradeRecovery(playerRock);
        UpdateUI();
    }
}