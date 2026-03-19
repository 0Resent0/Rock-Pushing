using UnityEngine;

public class RockController : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float tiltAmount = 30f;
    public float maxTilt = 45f;
    public float smoothSpeed = 5f;

    public float randomForce = 20f;     // strength of randomness
    public float randomSpeed = 2f;      // how fast it changes

    private float tilt = 0f;
    private float targetTilt = 0f;
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return;

        float mouseX = Input.GetAxis("Mouse X");

        if (Input.GetKey(KeyCode.W))
        {
            // Move forward
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime, Space.World);

            // Add smooth random tilt
            float randomTilt = Mathf.PerlinNoise(Time.time * randomSpeed, 0f) - 0.5f;
            targetTilt += randomTilt * randomForce * Time.deltaTime;
        }

        // Player correction
        targetTilt += -mouseX * tiltAmount * Time.deltaTime;

        // Clamp tilt
        targetTilt = Mathf.Clamp(targetTilt, -maxTilt, maxTilt);

        // Smooth movement
        tilt = Mathf.Lerp(tilt, targetTilt, smoothSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, 0, tilt);

        // Game Over
        if (Mathf.Abs(tilt) >= maxTilt)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over!");
    }
}