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
    public void Shopping()
    {
        SceneManager.LoadScene("FakeShop");
    }
    public void Upgrade()
    {
        SceneManager.LoadScene("Upgrade");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Map1()
    {
        SceneManager.LoadScene("Map 1");
    }
    public void Map2()
    {
        SceneManager.LoadScene("Map 2");
    }
    public void Map3()
    {
        SceneManager.LoadScene("Map 3");
    }
}