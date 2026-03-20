using UnityEngine;
using UnityEngine.UI;

public class RockController : MonoBehaviour
{
    [Header("Balance")]
    public float maxTilt = 45f;
    public float smoothSpeed = 5f;

    [Header("Rock Behavior (Tries to Lose)")]
    public float pullForce = 12f;
    public float switchTime = 2f;
    public float aggression = 1.5f;
    public float spikeChance = 0.02f;

    [Header("Control (Player Power)")]
    public float controlPower = 80f;
    public float edgeMultiplier = 2f;

    [Header("Recovery")]
    public float rockRestore = 0.5f;

    [Header("UI")]
    public RectTransform tiltIndicatorImage;

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

        // If NOT holding W → do nothing except optional restore
        if (!Input.GetKey(KeyCode.W))
        {
            if (rockRestore > 0f)
                targetTilt = Mathf.Lerp(targetTilt, 0f, rockRestore * Time.deltaTime);

            tilt = Mathf.Lerp(tilt, targetTilt, smoothSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, tilt);
            UpdateTiltIndicator();
            return;
        }

        // Switch direction
        switchTimer -= Time.deltaTime;
        if (switchTimer <= 0f)
        {
            loseDirection *= -1f;
            switchTimer = switchTime;
        }

        // Rock tries to fall
        float edgeFactor = Mathf.InverseLerp(0, maxTilt, Mathf.Abs(tilt));
        float force = pullForce * (1f + edgeFactor * aggression);
        targetTilt += loseDirection * force * Time.deltaTime;

        // Spike
        if (Random.value < spikeChance)
            targetTilt += loseDirection * force * 1.5f;

        // Anti-recovery
        targetTilt += tilt * 0.2f * Time.deltaTime;

        // ✅ PLAYER MOUSE CONTROL (restored)
        float input = Input.GetAxis("Mouse X");
        float control = controlPower * (1f + edgeFactor * edgeMultiplier);
        targetTilt += -input * control * Time.deltaTime;

        // Recovery (ONLY if value > 0)
        if (rockRestore > 0f)
            targetTilt = Mathf.Lerp(targetTilt, 0f, rockRestore * Time.deltaTime);

        // Clamp
        targetTilt = Mathf.Clamp(targetTilt, -maxTilt * 1.3f, maxTilt * 1.3f);

        // Smooth
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
        tiltIndicatorImage.localScale =
            Mathf.Abs(tilt) > maxTilt * 0.7f ? Vector3.one * 1.2f : Vector3.one;
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over!");
    }
}