using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalsackInteraction : Interactable
{
    public GameObject SackOnHand;
    public AudioSource SackAudio;

    private void FixedUpdate()
    {
        if (GameManager.Instance.fecthCoalTask)
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
        if (!SackOnHand.activeSelf && canInteract)
        {
            gameObject.SetActive(false);
            SackOnHand.SetActive(true);
            SackAudio.Play();
        }
    }
}
