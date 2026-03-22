using UnityEngine;
using TMPro;

public class TokenUI : MonoBehaviour
{
    public TextMeshProUGUI tokenText;

    void Update()
    {
        if (GameManager.Instance != null)
        {
            int tokens = GameManager.Instance.GetTokens();
            tokenText.text = "Total " + tokens + " Point";
        }
    }
}