using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startTime = 0f;        // Used if counting up
    public float countdownTime = 10f;   // Used if counting down
    public bool countDown = false;      // Toggle between count up / down
    public bool timerRunning = true;

    [Header("UI")]
    public TextMeshProUGUI timerText;

    private float currentTime;

    void Start()
    {
        // Initialize timer
        if (countDown)
            currentTime = countdownTime;
        else
            currentTime = startTime;
    }

    void Update()
    {
        if (!timerRunning) return;

        if (countDown)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime = 0;
                timerRunning = false;
                Debug.Log("Time's up!");
            }
        }
        else
        {
            currentTime += Time.deltaTime;
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Optional controls
    public void StartTimer()
    {
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void ResetTimer()
    {
        if (countDown)
            currentTime = countdownTime;
        else
            currentTime = startTime;
    }
}