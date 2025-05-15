using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimizer : MonoBehaviour
{
    public static bool belowDeck = false;
    public GameObject underTheDeck;

    private void FixedUpdate() {
        if (belowDeck) {
            underTheDeck.SetActive(true);
        }
        else {
            if (underTheDeck.activeSelf) {
                underTheDeck.SetActive(false);
            }
        }
    }
}
