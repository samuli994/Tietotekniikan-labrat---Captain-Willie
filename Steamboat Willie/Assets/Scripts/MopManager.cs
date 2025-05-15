using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MopManager : Interactable
{
    public GameObject brushOnHand;
    public GameObject brushOnDeck;
    public GameObject[] gameObjects;
    void Update()
    {
        if (cleaning) {
            if (gameObjects.All(gameObject => !gameObject.activeSelf)) {
                cleaning = false;
                brushOnHand.SetActive(false);
                brushOnDeck.SetActive(true);
                Debug.Log("Everything is now cleaned");
                GameManager.Instance.TaskDone();
            }
        }
        
    }

    public void ActivateGameObjects()
    {
        foreach (GameObject go in gameObjects)
        {
            go.SetActive(true);
            go.GetComponentInChildren<DirtInteract>().BringBackTransparency();
        }
    }
}
