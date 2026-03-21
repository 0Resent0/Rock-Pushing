using UnityEngine;
using UnityEngine.UI;

public class MapUnlockManager : MonoBehaviour
{
    public Button map1Button;
    public Button map2Button;

    void Start()
    {
        // 🔒 ล็อกก่อน
        map1Button.interactable = false;
        map2Button.interactable = false;

        // 🔓 ปลดล็อกตามข้อมูล
        if (PlayerPrefs.GetInt("UnlockMap1", 0) == 1)
            map1Button.interactable = true;

        if (PlayerPrefs.GetInt("UnlockMap2", 0) == 1)
            map2Button.interactable = true;
    }
}