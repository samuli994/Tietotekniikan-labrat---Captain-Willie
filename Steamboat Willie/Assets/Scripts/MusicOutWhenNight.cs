using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicOutWhenNight : MonoBehaviour
{
    public Light dirLight;
    public AudioSource audioSource;
    
    void FixedUpdate()
    {
        if (dirLight.intensity < 0.1f)
        {
            audioSource.mute = true;
        }
        else {

            audioSource.mute = false;
        }
    }
}
