using UnityEngine;
using UnityEngine.UI;

public class RockController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Balance Settings")]
    public float maxTilt = 45f;
    public float smoothSpeed = 5f;

    [Header("Rock Behavior")]
    public float pullForce = 12f;
    public float switchTime = 2f;
    public float aggression = 1.5f;

    [Header("Instability")]
    public float randomForce = 20f;
    public float randomSpeed = 2f;
    public float spikeChance = 0.03f;
    public float driftForce = 3f;

    [Header("Player Control")]
    public float mouseTiltPower = 30f;
    public float controlPower = 80f;
    public float edgeMultiplier = 2f;

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
    private bool started = false;

    void Start()
    {
        switchTimer = switchTime;
        loseDirection = Random.value > 0.5f ? 1f : -1f;
    }

    void Update()
    {
        if (isGameOver) return;

        // Start moving when W is pressed
        if (Input.GetKey(KeyCode.W))
        {
            started = true;

            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);

            // Rock tries to fall
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

            float mouseDelta = Input.GetAxis("Mouse X");
            targetTilt += -mouseDelta * controlPower * (1f + edgeFactor * edgeMultiplier) * Time.deltaTime;

            if (rockRestore > 0f)
                targetTilt = Mathf.Lerp(targetTilt, 0f, rockRestore * Time.deltaTime);
        }
        else if (rockRestore > 0f)
        {
            targetTilt = Mathf.Lerp(targetTilt, 0f, rockRestore * Time.deltaTime);
        }

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
    }
}