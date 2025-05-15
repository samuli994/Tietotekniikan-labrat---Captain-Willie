using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWillieSpawner : MonoBehaviour
{
    public GameObject willie;
    public bool firstTime = true;
    public bool spawnWillie = false;
    private bool coroutineGoing;
    public MonsterWillieSpawnerV2 monsterWillieSpawnerV2;

    private void FixedUpdate() {
        if (GameManager.Instance.currentDay == 8 && GameManager.Instance.underTheDeck)
        {
            willie.transform.localPosition = new Vector3(6.47800016f,-0.375999987f,-26.6569996f);
            willie.SetActive(false);
            spawnWillie = true;
        }
        if (GameManager.Instance.currentDay == 8 && !GameManager.Instance.underTheDeck && !willie.activeSelf && spawnWillie && firstTime) 
        {
            if (!coroutineGoing) 
            {
                coroutineGoing = true;
                StartCoroutine(delaySpawn());
            }
            
        }
    }

    private IEnumerator delaySpawn()
    {
        yield return new WaitForSeconds(4f);
        firstTime = false;
        monsterWillieSpawnerV2.spawnWillie = true;
        willie.SetActive(true);
        spawnWillie = false;
        coroutineGoing = false;
    }
}
