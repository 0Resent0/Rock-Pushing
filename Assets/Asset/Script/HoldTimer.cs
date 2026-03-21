using UnityEngine;
using TMPro;

public class HoldTimer : MonoBehaviour
{
    public float m = 0f; // ตัวแปรเวลา
    public TextMeshProUGUI displayText; // UI

    void Update()
    {
        // ถ้ากด Spacebar ค้าง
        if (Input.GetKey(KeyCode.Space))
        {
            m += Time.deltaTime;
        }

        // อัปเดต UI ทุกเฟรม
        displayText.text = "m: " + m.ToString("F1");
    }
}