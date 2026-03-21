using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footstep : MonoBehaviour
{
    public AudioSource footstepAudioSource;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            footstepAudioSource.enabled = true;
        }
        else
        {
            footstepAudioSource.enabled = false;
        }
    }
}
