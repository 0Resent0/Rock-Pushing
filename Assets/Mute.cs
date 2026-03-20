using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Mute : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private bool isMuted = false;

    public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;
    }
    void Update()
    {
        
    }
}
