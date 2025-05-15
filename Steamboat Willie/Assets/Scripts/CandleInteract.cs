using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleInteract : Interactable
{
    // Start is called before the first frame update
    public GameObject candleInHand;
    public bool hideDuringNight;

    private void FixedUpdate()
    {
        if ((GameManager.Instance.currentDay == 10 || GameManager.Instance.currentDay == 8) && hideDuringNight) {
            gameObject.SetActive(false);
        }
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
            candleInHand.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
