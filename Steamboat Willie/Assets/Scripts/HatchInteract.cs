using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchInteract : Interactable
{
    // Start is called before the first frame update
    public Animator anim;
    public AudioSource audioSource;
    public AudioClip openClip;
    public AudioClip closeClip;
    public bool open = false;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.hatchUnlocked)
        {
            canInteract = true;
        }
        else 
        {
            canInteract = false;
        }
    }

    public override void Interact()
    {
        if (canInteract)
        {
            anim.SetBool("Open", !open);
            open = !open;

            if (open)
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

    }
}
