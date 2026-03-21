using UnityEngine;
using TMPro;

public class HoldTimer : MonoBehaviour
{
    public float m = 0f;
    public TextMeshProUGUI displayText;

    public tpMapEnd mapEnd; // ลาก tpMapEnd มาใส่
    private bool triggered = false;
    public float Finish;

    void Update()
    {
        // กดค้าง Spacebar → เพิ่มค่า
        if (Input.GetKey(KeyCode.Space))
        {
            m += Time.deltaTime;
        }

        // แสดงค่า (ทศนิยม 1 ตำแหน่ง)
        displayText.text = m.ToString("F1") + "m";

        // ถ้าถึง 100 → สั่งจบ
        if (m >= Finish && !triggered)
        {
            triggered = true;
            Debug.Log("Timer reached 100!");

            mapEnd.TriggerFinish(); // เรียกอีก script
        }
    }
}