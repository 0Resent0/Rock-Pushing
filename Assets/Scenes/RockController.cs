using UnityEngine;
using UnityEngine.UI;

public class RockController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Balance Settings")]
    public float maxTilt = 55f;
    public float smoothSpeed = 8f;
    public float mouseTiltPower = 30f;
    public bool easyMode = false;      // Q/P keys if true
    public bool nightmareMode = false; // stronger instability

    [Header("Instability")]
    public float randomForce = 10f;
    public float randomSpeed = 1.5f;
    public float spikeChance = 0.03f;  // old-style occasional nudge
    public float driftForce = 1.5f;

    [Header("Recovery")]
    public float autoRecover = 2f;

    [Header("UI")]
    public RectTransform tiltIndicatorImage;
    public Color normalColor = Color.white;
    public Color warningColor = Color.red;
    [Range(0f, 1f)] public float warningThreshold = 0.8f;

    private float tilt = 0f;
    private float targetTilt = 0f;
    private bool isGameOver = false;
    private bool started = false;

    public bool IsGameOver => isGameOver;

    void Update()
    {
        if (isGameOver) return;

        // Start moving
        if (Input.GetKey(KeyCode.W))
            started = true;

        if (!started) return;

        // Forward movement
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);

        // Adjust parameters for Nightmare Mode
        float actualRandomForce = randomForce;
        float actualDriftForce = driftForce;
        float actualSpikeChance = spikeChance;

        if (nightmareMode)
        {
            actualRandomForce *= 2f;
            actualDriftForce *= 2f;
            actualSpikeChance *= 2f;
        }

        // Instability while moving
        if (Input.GetKey(KeyCode.W))
        {
            float randomTilt = (Mathf.PerlinNoise(Time.time * randomSpeed, 0f) - 0.5f) * actualRandomForce * Time.deltaTime;
            targetTilt += randomTilt;

            // OLD spike mechanic: nudge, does not instantly kill
            if (Random.value < actualSpikeChance)
            {
                float spike = Mathf.Sign(Random.value - 0.5f) * actualRandomForce * 1.5f;
                targetTilt += spike; // just moves tilt
                // optional: you could play a "shake" effect or sound here
            }

            targetTilt += actualDriftForce * Time.deltaTime;

            // Clamp slightly beyond maxTilt for spikes so they don't instantly kill
            float dangerMax = maxTilt * (nightmareMode ? 1.5f : 1.2f);
            targetTilt = Mathf.Clamp(targetTilt, -dangerMax, dangerMax);
        }

        // Player control
        float input = 0f;
        if (easyMode)
        {
            if (Input.GetKey(KeyCode.Q)) input = 1f;
            if (Input.GetKey(KeyCode.P)) input = -1f;
        }
        else
        {
            input = Input.GetAxisRaw("Mouse X");
        }

        targetTilt += -input * mouseTiltPower * Time.deltaTime;

        // Auto recovery
        targetTilt = Mathf.Lerp(targetTilt, 0f, autoRecover * Time.deltaTime);

        // Smooth tilt
        tilt = Mathf.Lerp(tilt, targetTilt, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, tilt);

        // Game Over check using actual tilt
        if (Mathf.Abs(tilt) >= maxTilt)
            GameOver();

        // Update tilt indicator
        UpdateTiltIndicator();
    }

    void UpdateTiltIndicator()
    {
        if (!tiltIndicatorImage) return;

        tiltIndicatorImage.localRotation = Quaternion.Euler(0, 0, tilt);

        float absTilt = Mathf.Abs(tilt);
        if (absTilt >= maxTilt * warningThreshold)
        {
            float t = (absTilt - maxTilt * warningThreshold) / (maxTilt * (1 - warningThreshold));
            tiltIndicatorImage.GetComponent<Image>().color = Color.Lerp(normalColor, warningColor, t);
            tiltIndicatorImage.localScale = Vector3.one * (1f + 0.2f * t);
        }
        else
        {
            tiltIndicatorImage.GetComponent<Image>().color = normalColor;
            tiltIndicatorImage.localScale = Vector3.one;
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over!");
    }
}