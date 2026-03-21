using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private bool tokenAwarded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (tokenAwarded) return;

        if (other.CompareTag("Player"))
        {
            tokenAwarded = true;
            Debug.Log("Player reached finish line!");

            // Award tokens based on scene configuration
            TokenManager.Instance.AwardToken();
        }
    }
}