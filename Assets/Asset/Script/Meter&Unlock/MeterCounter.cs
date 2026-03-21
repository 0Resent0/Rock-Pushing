using UnityEngine;
using TMPro;

public class MeterCounter : MonoBehaviour
{
    public float m = 0f;
    public TextMeshProUGUI displayText;

    public tpMapEnd mapEnd;
    private bool triggered = false;
    public float Finish;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            m += Time.deltaTime;
        }

        displayText.text = m.ToString("F1") + "m";

        if (m >= Finish && !triggered)
        {
            triggered = true;
            Debug.Log("Win!");

            mapEnd.TriggerFinish();

            // 🔢 นับจำนวนครั้งที่ชนะ
            int winCount = PlayerPrefs.GetInt("WinCount", 0);
            winCount++;
            PlayerPrefs.SetInt("WinCount", winCount);

            // 🔓 ปลดล็อกตามรอบ
            if (winCount >= 1)
                PlayerPrefs.SetInt("UnlockMap1", 1);

            if (winCount >= 2)
                PlayerPrefs.SetInt("UnlockMap2", 1);

            PlayerPrefs.Save();
        }
    }
}