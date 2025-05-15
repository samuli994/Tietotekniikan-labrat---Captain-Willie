using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSFXManager : MonoBehaviour
{
    private AudioSource[] audioSources;
    [SerializeField] private int minimunWaitTime = 5;
    [SerializeField] private int maximumWaitTime = 30;

    [SerializeField] private AudioClip[] audioClips;

    private bool activated = false;
    public bool playAudio = true;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize all children audio sources to list
        audioSources = GetComponentsInChildren<AudioSource>();

        // disable looping for the sources
        foreach (AudioSource source in audioSources)
        {
            source.loop = false;
        }
        StartCoroutine(PlayRandomAudio());
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.underTheDeck && !activated)
        {
            StartCoroutine(PlayRandomAudio());
            activated = true;
        }
        else if (!GameManager.Instance.underTheDeck)
        {
            activated = false;
        }
    }

    IEnumerator PlayRandomAudio()
    {
        while (GameManager.Instance.underTheDeck)
        {
            int sourceIndex = GetRandomValue(0, audioSources.Length);
            int timeOffSet = GetRandomValue(minimunWaitTime, maximumWaitTime + 1); // +1 to take into account the Random.Range exclusive max value
            int clipIndex = GetRandomValue(0, audioClips.Length);

            // get random source and clip
            AudioSource chosenSource = audioSources[sourceIndex];
            AudioClip chosenClip = audioClips[clipIndex];

            // set the clip for the source
            chosenSource.clip = chosenClip;
            chosenSource.Play();
            Debug.Log("played some audio");
            // wait for a minimum of 5 and maximum of 5 + timeOffset seconds
            yield return new WaitForSecondsRealtime(chosenClip.length);
            // This is kinda weird but should(?) work
            yield return new WaitForSeconds(timeOffSet);
        }
    }

    int GetRandomValue(int low, int high)
    {
        return Random.Range(low, high);
    }
}
