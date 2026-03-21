using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    public AudioSource fadeSound;

    void Start()
    {
        
    }

    public void FadeToScene(string sceneName)
    {
        fadeSound.Play();

        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeIn()
    {
        fadeSound.Play();

        float t = fadeDuration;

        while (t > 0)
        {
            t -= Time.deltaTime;
            float alpha = t / fadeDuration;
            fadeImage.color = new Color(255, 255, 255, alpha);
            yield return null;
        }
    }

    IEnumerator FadeOut(string sceneName)
    {
        fadeSound.Play();

        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = t / fadeDuration;
            fadeImage.color = new Color(255, 255, 255, alpha);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
