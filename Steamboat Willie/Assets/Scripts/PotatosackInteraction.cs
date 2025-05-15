using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatosackInteraction : Interactable
{
    public GameObject SackOnHand;

    public AudioSource SackAudio;

    void FixedUpdate()
    {
        if (GameManager.Instance.fecthPotatoesTask)
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
        if (!SackOnHand.activeSelf && GameManager.Instance.fecthPotatoesTask)
        {
            gameObject.SetActive(false);
            SackOnHand.SetActive(true);
            SackAudio.Play();
        }
    }
}
