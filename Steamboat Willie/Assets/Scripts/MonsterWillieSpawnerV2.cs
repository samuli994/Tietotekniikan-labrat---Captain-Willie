using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWillieSpawnerV2 : MonoBehaviour
{
    public GameObject willie;
    public bool firstTime;
    public bool spawnWillie = false;
    public bool spawnChaserWillie = false;
    private bool coroutineGoing;

    public void SpawnWillieUnderTheDeck() 
    {
        if (GameManager.Instance.currentDay == 8 && GameManager.Instance.underTheDeck && spawnWillie)
        {
            StartCoroutine(delaySpawn());
        }
    }

    private IEnumerator delaySpawn()
    {
        yield return new WaitForSeconds(4f);
        willie.SetActive(true);
        spawnWillie = false;
        coroutineGoing = false;
        spawnChaserWillie = true;
    }
}
