using UnityEngine;
using UnityEngine.UI;

public class RockController : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 5f;

    [Header("Balance Settings")]
    public float tiltAmount = 30f;  // how strongly mouse movement adds to tilt
    public float maxTilt = 45f;
    public float smoothSpeed = 5f;

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

        // Start forward movement when W pressed
        if (Input.GetKey(KeyCode.W))
        {
            started = true;
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime, Space.World);

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
            // Get mouse movement delta (frame-to-frame)
            float mouseDelta = Input.GetAxis("Mouse X");

            // Add the movement effect to tilt
            targetTilt += -mouseDelta * tiltAmount * Time.deltaTime;
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