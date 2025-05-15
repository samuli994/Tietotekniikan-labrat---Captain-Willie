using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HornInteract : Interactable
{
    // Start is called before the first frame update
    private AudioSource audioSource;
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        canInteract = true;
    }

    public override void Interact()
    {
        audioSource.Play();
    }
}
