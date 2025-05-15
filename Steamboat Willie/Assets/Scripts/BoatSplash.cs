using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSplash : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) return; // no splash for player
        GetComponent<AudioSource>().Play();
    }
}
