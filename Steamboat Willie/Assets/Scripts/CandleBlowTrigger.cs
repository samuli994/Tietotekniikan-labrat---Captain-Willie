using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleBlowTrigger : MonoBehaviour
{
    public WillieAi willieAi;
    public Transform ogPosition;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.currentDay == 8) 
            {
                //willieAi.StopPlayerActions();
                other.transform.position = ogPosition.position;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.currentDay == 8) 
            {
                willieAi.onTrigger = true;
                ogPosition = other.transform;
            }
        }
    }
}
