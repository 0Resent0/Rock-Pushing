using UnityEngine;

public class RockFollower : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform rock;
    public Vector3 followOffset = new Vector3(0f, 1f, -2f);
    public float followSpeed = 5f;

    [Header("Movement Tracking")]
    private Vector3 lastRockPosition;
    private Vector3 moveDirection;

    [Header("Animator")]
    public Animator animator;

    void Start()
    {
        if (rock)
            lastRockPosition = rock.position;
    }

    void Update()
    {
        if (!rock) return;

        // ✅ Calculate movement direction (ignores rotation completely)
        Vector3 delta = rock.position - lastRockPosition;

        if (delta.magnitude > 0.001f)
        {
            moveDirection = delta.normalized;
        }

        lastRockPosition = rock.position;

        // If no movement yet, default forward
        if (moveDirection == Vector3.zero)
            moveDirection = Vector3.forward;

        // Get right direction from movement
        Vector3 right = Vector3.Cross(Vector3.up, moveDirection).normalized;

        // Calculate target position
        Vector3 targetPos = rock.position
                          + right * followOffset.x
                          + Vector3.up * followOffset.y
                          - moveDirection * followOffset.z;

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);

        // ❌ No rotation from rock

        // Animation
        if (animator)
        {
            bool isMoving = delta.magnitude > 0.01f;
            animator.SetBool("isRunning", isMoving);
        }
    }
}