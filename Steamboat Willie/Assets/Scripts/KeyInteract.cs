using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteract : Interactable
{
    // Update is called once per frame
    public MonsterWillieSpawnerV2 monsterWillieSpawnerV2;
    public AudioSource audioSource;
    public AudioSource keyPickUp;
    void FixedUpdate()
    {
        if (GameManager.Instance.currentDay == 8)
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
            keyPickUp.Play();
            GameManager.Instance.hasKey = true;
            audioSource.Play();
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(spawnWithDelay());
        }
    }

    public IEnumerator spawnWithDelay() {
        yield return new WaitForSeconds(2f);
        monsterWillieSpawnerV2.SpawnWillieUnderTheDeck();
    }

}
