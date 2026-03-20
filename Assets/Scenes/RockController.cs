using UnityEngine;
using UnityEngine.UI;

public class RockController : MonoBehaviour
{
    [Header("Settings")]
    public bool keyboardMode = true;   // ✅ renamed
    public bool fatigueMode = true;    // ✅ NEW toggle

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rollSpeed = 200f;

    [Header("Balance Settings")]
    public float maxTilt = 45f;
    public float smoothSpeed = 5f;
    public float mouseTiltPower = 30f;
    public float keyTiltPower = 40f;

    [Header("Rock Behavior")]
    public float pullForce = 12f;
    public float switchTime = 2f;
    public float aggression = 1.5f;

    [Header("Instability")]
    public float randomForce = 20f;
    public float randomSpeed = 2f;
    public float spikeChance = 0.03f;
    public float driftForce = 3f;

    [Header("Recovery")]
    public float rockRestore = 0.5f;

    [Header("UI")]
    public RectTransform tiltIndicatorImage;

    [Header("Tilt Indicator Settings")]
    [Range(0f, 1f)]
    public float colorChangePercent = 0.8f;

    [Header("Advanced Control")]

    [Header("Fatigue")]
    public float fatigueIncrease = 2f;
    public float fatigueRecovery = 1f;
    public float maxFatigue = 1f;

    [Header("Input Weight")]
    public float inputSmoothTime = 0.15f;

    private float fatigue = 0f;
    private float currentInput = 0f;
    private float inputVelocity = 0f;
    private float lastRawInput = 0f;

    private float tilt = 0f;
    private float targetTilt = 0f;
    private float loseDirection = 1f;
    private float switchTimer;
    private bool isGameOver = false;

    void Start()
    {
        switchTimer = switchTime;
        loseDirection = Random.value > 0.5f ? 1f : -1f;
    }

    void Update()
    {
        if (isGameOver) return;

        bool moving = Input.GetKey(KeyCode.Space);

        // =========================
        // 🎮 INPUT SYSTEM
        // =========================

        float rawInput = 0f;

        if (keyboardMode) // ✅ renamed usage
        {
            if (Input.GetKey(KeyCode.Q)) rawInput = 1f;
            else if (Input.GetKey(KeyCode.P)) rawInput = -1f;
        }
        else
        {
            rawInput = Input.GetAxis("Mouse X");
        }

        // =========================
        // 🧠 FATIGUE SYSTEM (toggleable)
        // =========================

        if (fatigueMode)
        {
            // Detect spam
            if (Mathf.Sign(rawInput) != Mathf.Sign(lastRawInput) && Mathf.Abs(rawInput) > 0.5f)
            {
                fatigue += fatigueIncrease * Time.deltaTime;
            }

            // Recover fatigue
            fatigue -= fatigueRecovery * Time.deltaTime;
            fatigue = Mathf.Clamp(fatigue, 0f, maxFatigue);
        }
        else
        {
            fatigue = 0f; // no fatigue if disabled
        }

        lastRawInput = rawInput;

        // =========================
        // 🪨 INPUT WEIGHT
        // =========================

        currentInput = Mathf.SmoothDamp(currentInput, rawInput, ref inputVelocity, inputSmoothTime);

        float fatigueMultiplier = 1f - fatigue;
        float finalInput = currentInput * fatigueMultiplier;

        // =========================
        // 🪨 MOVEMENT + ROCK LOGIC
        // =========================

        if (moving)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.right, rollSpeed * Time.deltaTime);

            switchTimer -= Time.deltaTime;
            if (switchTimer <= 0f)
            {
                loseDirection *= -1f;
                switchTimer = switchTime;
            }

            float edgeFactor = Mathf.InverseLerp(0, maxTilt, Mathf.Abs(tilt));
            float force = pullForce * (1f + edgeFactor * aggression);
            targetTilt += loseDirection * force * Time.deltaTime;

            if (Random.value < spikeChance)
                targetTilt += loseDirection * force * 1.5f;

            targetTilt += tilt * 0.2f * Time.deltaTime;

            float randomTilt = Mathf.PerlinNoise(Time.time * randomSpeed, 0f) - 0.5f;
            targetTilt += randomTilt * randomForce * Time.deltaTime;

            if (Random.value < spikeChance)
                targetTilt += Random.Range(-1f, 1f) * randomForce * 2f;

            targetTilt += driftForce * Time.deltaTime;
        }
        else
        {
            targetTilt = Mathf.Lerp(targetTilt, 0f, rockRestore * Time.deltaTime);
        }

        // =========================
        // 🎯 APPLY CONTROL
        // =========================

        float controlPower = keyboardMode ? keyTiltPower : mouseTiltPower;
        float edgeFactorPlayer = Mathf.InverseLerp(0, maxTilt, Mathf.Abs(tilt));

        targetTilt += -finalInput * controlPower * (1f + edgeFactorPlayer) * Time.deltaTime;

        // =========================
        // 🎯 APPLY ROTATION
        // =========================

        targetTilt = Mathf.Clamp(targetTilt, -maxTilt * 1.3f, maxTilt * 1.3f);
        tilt = Mathf.Lerp(tilt, targetTilt, smoothSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, tilt);

        if (Mathf.Abs(tilt) >= maxTilt)
            GameOver();

        UpdateTiltIndicator();
    }

    void UpdateTiltIndicator()
    {
        if (!tiltIndicatorImage) return;

        tiltIndicatorImage.localRotation = Quaternion.Euler(0, 0, tilt);

        Image indicatorImage = tiltIndicatorImage.GetComponent<Image>();
        if (indicatorImage)
        {
            float tiltPercent = Mathf.Abs(tilt) / maxTilt;
            indicatorImage.color = tiltPercent > colorChangePercent ? Color.red : Color.white;
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over!");
    }
}