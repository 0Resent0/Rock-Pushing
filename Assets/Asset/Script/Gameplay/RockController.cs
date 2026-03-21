using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RockController : MonoBehaviour
{
    [Header("Settings")]
    public bool keyboardMode = true; // true = Q/P keys, false = mouse X axis

    [Header("Movement")]
    public float moveSpeed = 5f;

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

    [Header("Fatigue System")]
    public bool fatigueMode = true;     // true = control weakens if spammed
    public float fatigueRate = 5f;      // how fast fatigue accumulates
    public float fatigueRecover = 2f;   // how fast fatigue recovers
    private float fatigueAmount = 0f;   // 0 = full control, 1 = no control

    [Header("Stuck System")]
    public float stuckChance = 0.05f;
    public float unstuckThreshold = 3f;
    public RectTransform stuckIndicator;
    public Image stuckProgressBar;

    [Header("UI")]
    public RectTransform tiltIndicatorImage;
    [Range(0f, 1f)]
    public float colorChangePercent = 0.8f;

    private float tilt = 0f;
    private float targetTilt = 0f;
    private float loseDirection = 1f;
    private float switchTimer;
    private bool isGameOver = false;

    private bool isStuck = false;
    private float stuckProgress = 0f;

    void Start()
    {
        switchTimer = switchTime;
        loseDirection = Random.value > 0.5f ? 1f : -1f;
        tilt = 0f;
        targetTilt = 0f;
        if (stuckIndicator) stuckIndicator.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        bool moving = Input.GetKey(KeyCode.Space);

        // Random chance to get stuck while moving
        if (moving && !isStuck)
        {
            if (Random.value < stuckChance * Time.deltaTime)
            {
                isStuck = true;
                stuckProgress = 0f;
                if (stuckIndicator) stuckIndicator.gameObject.SetActive(true);
            }
        }

        // Handle stuck mechanics
        if (isStuck)
        {
            float input = 0f;
            if (keyboardMode)
            {
                if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.P)) input = 1f;
            }
            else
            {
                input = Mathf.Abs(Input.GetAxis("Mouse X"));
            }

            stuckProgress += input * Time.deltaTime * 10f;

            if (stuckProgressBar)
                stuckProgressBar.fillAmount = Mathf.Clamp01(stuckProgress / unstuckThreshold);

            if (stuckProgress >= unstuckThreshold)
            {
                isStuck = false;
                stuckProgress = 0f;
                if (stuckIndicator) stuckIndicator.gameObject.SetActive(false);
            }

            // Apply fatigue when stuck
            if (fatigueMode)
            {
                fatigueAmount = Mathf.Clamp01(fatigueAmount + input * Time.deltaTime * fatigueRate);
            }
        }

        // Movement and tilt only if not stuck
        if (moving && !isStuck)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);

            // Automatic tilt logic
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

        // Smooth return to upright when not moving
        if (!moving && rockRestore > 0f)
            targetTilt = Mathf.Lerp(targetTilt, 0f, rockRestore * Time.deltaTime);

        // Player input
        float playerInput = 0f;
        if (keyboardMode)
        {
            if (Input.GetKey(KeyCode.Q)) playerInput = 1f;
            else if (Input.GetKey(KeyCode.P)) playerInput = -1f;
        }
        else
        {
            playerInput = Input.GetAxis("Mouse X");
        }

        // Apply fatigue effect
        if (fatigueMode)
        {
            fatigueAmount = Mathf.Clamp01(fatigueAmount - Time.deltaTime * fatigueRecover);
            playerInput *= (1f - fatigueAmount);
        }

        float controlPower = keyboardMode ? keyTiltPower : mouseTiltPower;
        float edgeFactorPlayer = Mathf.InverseLerp(0, maxTilt, Mathf.Abs(tilt));
        targetTilt += -playerInput * controlPower * (1f + edgeFactorPlayer) * Time.deltaTime;

        // Clamp and smooth tilt
        targetTilt = Mathf.Clamp(targetTilt, -maxTilt * 1.3f, maxTilt * 1.3f);
        tilt = Mathf.Lerp(tilt, targetTilt, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, tilt);

        if (Mathf.Abs(tilt) >= maxTilt)
            GameOver();

        UpdateTiltIndicator();
    }

    void UpdateTiltIndicator()
    {
        if (!tiltIndicatorImage) return;

        tiltIndicatorImage.localRotation = Quaternion.Euler(0, 0, tilt);
        tiltIndicatorImage.localScale = Mathf.Abs(tilt) > maxTilt * 0.7f ? Vector3.one * 1.2f : Vector3.one;

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
        SceneManager.LoadScene("Gameover");
    }
}