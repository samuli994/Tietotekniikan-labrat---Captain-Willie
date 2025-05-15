using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SackTrigger : MonoBehaviour
{
    public GameObject SackOnHand;
    public bool potatoRoom = false;

    public AudioSource SackAudio;

    public GameObject sackOnTheRoom;
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") 
        {
            if (SackOnHand.activeSelf) {
                if (GameManager.Instance.fecthPotatoesTask && potatoRoom) {
                    SackOnHand.SetActive(false);
                    sackOnTheRoom.SetActive(true);
                    //TODO: PERUNAT LISÄÄNTYY
                    SackAudio.Play();
                    GameManager.Instance.TaskDone();
                }
                if (GameManager.Instance.fecthCoalTask && !potatoRoom) {
                    SackOnHand.SetActive(false);
                    sackOnTheRoom.SetActive(true);
                    SackAudio.Play();
                    //TODO: Coal LISÄÄNTYY
                    GameManager.Instance.TaskDone();
                }
            }
            
        }
    }
}
