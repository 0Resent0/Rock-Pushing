using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tpMapEnd : MonoBehaviour
{
    public Collider otherCollider;
    [SerializeField] string nextSceneName;
    public SceneFader fader;

    void Update()
    {
        if (GetComponent<Collider>().bounds.Intersects(otherCollider.bounds))
        {
            Debug.Log("Collided!");

            fader.FadeToScene(nextSceneName);
        }
    }
}
