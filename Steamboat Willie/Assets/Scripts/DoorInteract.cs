using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : Interactable
{
    // Start is called before the first frame update
    private Animator anim;
    public bool doorOpen;

    public AudioSource audioSource;
    public AudioClip openClip;
    public AudioClip closeClip;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        doorOpen = anim.GetBool("DoorOpen");
        canInteract = true;
    }

    public override void Interact()
    {
        anim.SetBool("DoorOpen", !doorOpen);
        doorOpen = !doorOpen;

        if (doorOpen)
        {
            audioSource.clip = openClip;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = closeClip;
            audioSource.Play();
        }
    }

    public void ShutTheDoor()
    {
        anim.SetBool("DoorOpen", false);
        doorOpen = false;

        if (!doorOpen)
        {
            audioSource.clip = closeClip;
            audioSource.Play();
        }
    }
}
