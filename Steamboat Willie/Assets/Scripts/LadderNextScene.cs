using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LadderNextScene : Interactable
{
    public GameObject fpc;
    public bool underTheDeck = false;
    public GameObject candleStickOnHand;
    public GameObject candleStickOnBarrel;
    public PotatoSackManager potatoSackManager;
    public AudioSource audioSource;
    private bool coroutineStarted = false;

    public List<AudioSource> audios;

    public Fade fader;


    void Start()
    {
        fpc = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (candleStickOnHand.activeSelf)
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
        if (candleStickOnHand.activeSelf)
        {
            if (GameManager.Instance.currentDay == 8)
            {
                Ladders();
            }
            else if (!coroutineStarted) 
            {
                StartCoroutine(Fader());
            }
            
        }
    }

    public void Ladders()
    {
        if (!underTheDeck)
        {
            MuteAllAudios();
            Optimizer.belowDeck = true;
            fpc.transform.SetPositionAndRotation(new Vector3(-0.168f, -10.906f, -24.1f), fpc.transform.rotation);
            if (GameManager.Instance.currentTask == "EtsiPerunaa")
            {
                potatoSackManager.HidePotatoSacks();
                GameManager.Instance.TaskDone();
            }
        }
        else
        {
            UnmuteAllAudios();
            fpc.transform.SetPositionAndRotation(new Vector3(3.073f, 4.403f, -17.21f), fpc.transform.rotation);
            Optimizer.belowDeck = false;
            if (GameManager.Instance.currentDay != 10 && GameManager.Instance.currentDay != 8) {
                candleStickOnHand.SetActive(false);
                candleStickOnBarrel.SetActive(true);
            }
        }
    }

    private void MuteAllAudios()
    {
        foreach (var audioSource in audios)
        {
            audioSource.mute = true;
        }
    }

    private void UnmuteAllAudios()
    {
        foreach (var audioSource in audios)
        {
            audioSource.mute = false;
        }
    }

    public IEnumerator Fader()
    {
        coroutineStarted = true;
        audioSource.Play();
        fader.FadeIn(1);
        yield return new WaitForSeconds(2f);
        fader.FadeOut(1);
        Ladders();
        coroutineStarted = false;
    }
}
