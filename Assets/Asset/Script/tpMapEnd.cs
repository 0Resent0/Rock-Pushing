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
        if (triggered) return;

        triggered = true;
        Debug.Log("Finish triggered!");

        // ให้ Token
        TokenManager.Instance.AwardToken();

        // เปลี่ยนฉาก
        fader.FadeToScene(nextSceneName);
    }
}