using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Chapter");
    }
    public void Back()
    {
        SceneManager.LoadScene("Mainmenu");
    }
    public void Setting()
    {
        SceneManager.LoadScene("Setting");
    }

    public void Upgrade()
    {
        SceneManager.LoadScene("Upgrade");
    }
    public void Quit()
    {
        Application.Quit();
    }
}