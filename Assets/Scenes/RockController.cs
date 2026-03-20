using UnityEngine;
using UnityEngine.UI;

public class RockController : MonoBehaviour
{
    [Header("Settings")]
    public bool easyMode = true; // true = Q/P keys, false = mouse X axis

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

    [Header("UI")]
    public RectTransform tiltIndicatorImage;

    [Header("Tilt Indicator Settings")]
    [Range(0f, 1f)]
    public float colorChangePercent = 0.8f; // Threshold % for changing color

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

        // Rock tries to fall automatically
        switchTimer -= Time.deltaTime;
        if (switchTimer <= 0f)
        {
            loseDirection *= -1f;
            switchTimer = switchTime;
        }

        float edgeFactor = Mathf.InverseLerp(0, maxTilt, Mathf.Abs(tilt));
        float force = pullForce * (1f + edgeFactor * aggression);
        targetTilt += loseDirection * force * Time.deltaTime;

        // Random spike
        if (Random.value < spikeChance)
            targetTilt += loseDirection * force * 1.5f;

        // Anti-recovery
        targetTilt += tilt * 0.2f * Time.deltaTime;

        // Smooth random instability
        float randomTilt = Mathf.PerlinNoise(Time.time * randomSpeed, 0f) - 0.5f;
        targetTilt += randomTilt * randomForce * Time.deltaTime;

        // Sudden spikes
        if (Random.value < spikeChance)
            targetTilt += Random.Range(-1f, 1f) * randomForce * 2f;

        // Constant drift
        targetTilt += driftForce * Time.deltaTime;

        // Recovery
        if (rockRestore > 0f)
            targetTilt = Mathf.Lerp(targetTilt, 0f, rockRestore * Time.deltaTime);

        // ✅ Player Control
        float input = 0f;
        if (easyMode)
        {
            // Q and P keys control tilt
            if (Input.GetKey(KeyCode.Q)) input = 1f;
            else if (Input.GetKey(KeyCode.P)) input = -1f;
        }
        else
        {
            // Mouse X-axis control
            input = Input.GetAxis("Mouse X");
        }

        // Apply input to target tilt
        float controlPower = easyMode ? keyTiltPower : mouseTiltPower;
        targetTilt += -input * controlPower * (1f + edgeFactor) * Time.deltaTime;

        // Clamp and smooth tilt
        targetTilt = Mathf.Clamp(targetTilt, -maxTilt * 1.3f, maxTilt * 1.3f);
        tilt = Mathf.Lerp(tilt, targetTilt, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, tilt);

        // Check Game Over
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
    }
}