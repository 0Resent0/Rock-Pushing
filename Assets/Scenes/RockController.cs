using UnityEngine;
using UnityEngine.UI;

public class RockController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;         // Forward movement speed

    [Header("Balance Settings")]
    public float maxTilt = 45f;          // Max tilt before Game Over
    public float smoothSpeed = 5f;       // Tilt smoothing speed
    public float mouseTiltPower = 30f;   // How strongly mouse movement affects tilt

    [Header("Instability")]
    public float randomForce = 20f;
    public float randomSpeed = 2f;
    public float spikeChance = 0.03f;
    public float driftForce = 3f;

    [Header("UI")]
    public RectTransform tiltIndicatorImage;

    private float tilt = 0f;
    private float targetTilt = 0f;
    private bool isGameOver = false;
    private bool started = false;

    void Update()
    {
        if (isGameOver) return;

        // Start moving forward when W pressed
        if (Input.GetKey(KeyCode.W))
        {
            started = true;

            // Forward movement
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);

            // Add smooth random instability
            float randomTilt = Mathf.PerlinNoise(Time.time * randomSpeed, 0f) - 0.5f;
            targetTilt += randomTilt * randomForce * Time.deltaTime;

            // Sudden spikes
            if (Random.value < spikeChance)
            {
                float spike = Random.Range(-1f, 1f) * randomForce * 2f;
                targetTilt += spike;
            }

            // Constant drift
            targetTilt += driftForce * Time.deltaTime;
        }

        // Mouse movement adds to tilt
        if (started)
        {
            float mouseDelta = Input.GetAxis("Mouse X");
            targetTilt += -mouseDelta * mouseTiltPower * Time.deltaTime;
        }

        // Smooth tilt visually
        tilt = Mathf.Lerp(tilt, targetTilt, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, tilt);

        // Check Game Over
        if (Mathf.Abs(tilt) >= maxTilt)
        {
            GameOver();
        }

        // Update tilt indicator UI
        UpdateTiltIndicator();
    }

    void UpdateTiltIndicator()
    {
        if (!tiltIndicatorImage) return;
        tiltIndicatorImage.localRotation = Quaternion.Euler(0, 0, tilt);
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over!");
    }
}