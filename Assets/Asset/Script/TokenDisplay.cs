using UnityEngine;
using TMPro;

public class TotalTokenDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text tokenText;

    private void Awake()
    {
        if (tokenText == null)
            tokenText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        if (TokenManager.Instance != null)
            TokenManager.Instance.OnTotalTokensUpdated += UpdateTokenText;

        // Initialize display
        if (TokenManager.Instance != null)
            UpdateTokenText(TokenManager.Instance.TotalTokens);
    }

    private void OnDisable()
    {
        if (TokenManager.Instance != null)
            TokenManager.Instance.OnTotalTokensUpdated -= UpdateTokenText;
    }

    private void UpdateTokenText(int totalTokens)
    {
        tokenText.text = totalTokens + " points"; // e.g., "5 points"
    }
}