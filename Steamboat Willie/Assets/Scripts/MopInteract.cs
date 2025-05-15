using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MopInteract : Interactable
{
    public GameObject brushOnHand;

    private void FixedUpdate()
    {
        if (GameManager.Instance.cleanTask)
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
            brushOnHand.SetActive(true);
            cleaning = true;
            gameObject.SetActive(false);
            brushOnHand.GetComponent<AudioSource>().Play();
        }
    }
}
