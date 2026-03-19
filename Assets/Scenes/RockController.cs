using UnityEngine;

public class RockController : MonoBehaviour
{
    public float forwardSpeed = 5f;
    public float balanceSpeed = 3f;
    public float tiltAmount = 20f;

    private float tilt = 0f;

    void Update()
    {
        // Move forward constantly
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        // Left / Right input (A/D or Arrow keys)
        float input = Input.GetAxis("Horizontal");

        // Move left/right
        transform.Translate(Vector3.right * input * balanceSpeed * Time.deltaTime);

        // Tilt the rock (visual balance effect)
        tilt = Mathf.Lerp(tilt, -input * tiltAmount, 5f * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0, 0, tilt);
    }
}