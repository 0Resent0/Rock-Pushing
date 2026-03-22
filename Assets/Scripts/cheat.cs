using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cheat : MonoBehaviour
{
    public Button map1Button;
    public Button map2Button;

    public void getCheat()
    {
        Debug.Log("cheated.");
        map1Button.interactable = true;
        map2Button.interactable = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddTokens(100);
            Debug.Log("Add token");
        }
        else
        {
            Debug.LogWarning("GameManager instance not found!");
        }
    }
}
