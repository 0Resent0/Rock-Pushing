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
        SceneManager.LoadScene("Ads 1");
    }
    public void Setting()
    {
        SceneManager.LoadScene("Setting");
    }
    public void Shopping()
    {
        SceneManager.LoadScene("FakeShop");
    }
    public void Skin()
    {
        SceneManager.LoadScene("Skin");
    }
    public void Upgrade()
    {
        SceneManager.LoadScene("Upgrade");
    }
    public void Credit()
    {
        SceneManager.LoadScene("Credit");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Map1()
    {
        SceneManager.LoadScene("Map1 start");
    }
    public void Map2()
    {
        SceneManager.LoadScene("Map2 start");
    }
    public void Map3()
    {
        SceneManager.LoadScene("Map3 start");
    }
}