using UnityEngine;

public class tpMapEnd : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private SceneFader fader;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            TriggerFinish();
        }
    }
    public void TriggerFinish()
    {
        triggered = true;
        Debug.Log("Finish triggered!");

        if (GameManager.Instance != null)
            GameManager.Instance.AddTokens(1);
        else
            Debug.LogWarning("GameManager instance not found!");

        if (fader != null)
            fader.FadeToScene(nextSceneName);
        else
            Debug.LogWarning("Fader not assigned!");
    }
}