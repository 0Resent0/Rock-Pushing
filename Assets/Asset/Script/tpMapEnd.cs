using UnityEngine;

public class tpMapEnd : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private SceneFader fader;

    private bool triggered = false; // Prevent multiple triggers

    private void OnTriggerEnter(Collider other)
    {
        // Only trigger for the player
        if (triggered) return;

        if (other.CompareTag("Player")) // Make sure your player has the "Player" tag
        {
            triggered = true;
            Debug.Log("Player reached finish line!");

            // Award token
            TokenManager.Instance.AwardToken();

            // Fade to next scene
            fader.FadeToScene(nextSceneName);
        }
    }
}