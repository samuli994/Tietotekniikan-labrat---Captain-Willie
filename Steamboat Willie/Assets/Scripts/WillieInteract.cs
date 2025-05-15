using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillieInteract : Interactable
{
    public SpeechManager speech;

    public AudioSource audioSource;
    public List<AudioClip> clips;
    private AudioClip lastPlayedClip;
    public SteeringInteract steeringInteract;
    private FirstPersonController fpc;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fpc = player.GetComponent<FirstPersonController>();
        canInteract = true;
    }

    public override void Interact()
    {
        if (GameManager.Instance.driveTask && steeringInteract.steeringDone)
        {
            steeringInteract.driving = false;
            steeringInteract.steeringDone = false;
            fpc.enableSprint = true;
            fpc.playerCanMove = true;
            fpc.enableHeadBob = true;
            fpc.enableJump = true;
            GameManager.Instance.TaskDone();
        }

        speech.ShowText(GameManager.Instance.currentDay, GameManager.Instance.currentTask);
        audioSource.clip = chooseClip();
        audioSource.Play();
        if (GameManager.Instance.talkTask)
        {
            // WILLIE JAUHAA JOTAI JA SIT DONE
            GameManager.Instance.TaskDone();
        }

    }

    public AudioClip chooseClip()
    {
        if (clips.Count == 0)
        {
            Debug.LogWarning("No audio clips available.");
            return null;
        }
        // Get a random index different from the last played clip
        int randomIndex = Random.Range(0, clips.Count);
        while (clips[randomIndex] == lastPlayedClip)
        {
            randomIndex = Random.Range(0, clips.Count);
        }
        lastPlayedClip = clips[randomIndex];
        return lastPlayedClip;
    }
}