using UnityEngine;

public class RockFollower : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform rock;               // Assign your rock here
    public Vector3 followOffset = new Vector3(0f, 1f, -2f); // X, Y, Z offset from rock
    public float followSpeed = 5f;       // How quickly it follows
    public float rotateSpeed = 10f;      // How smoothly it rotates

    [Header("Animator")]
    public Animator animator;            // Assign your model's Animator

    void Update()
    {
        if (!rock) return;

        // Compute target position using configurable offset
        Vector3 targetPos = rock.position
                          + rock.right * followOffset.x
                          + Vector3.up * followOffset.y
                          + rock.forward * followOffset.z;

        // Smooth position follow
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);

        // Smooth rotation to face the same direction as rock
        Quaternion targetRot = Quaternion.LookRotation(rock.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);

        // Play animation if moving
        if (animator)
        {
            bool isMoving = Vector3.Distance(transform.position, targetPos) > 0.01f;
            animator.SetBool("isRunning", isMoving); // make sure your Animator has "isRunning" bool
        }
    }
}