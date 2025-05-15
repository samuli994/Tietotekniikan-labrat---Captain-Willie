using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Shovel : MonoBehaviour
{
    public bool hasCoal = false;
    private int coalFed = 0;
    private CoalInteraction ci;
    private bool shovelMoving = false;
    [SerializeField] private GameObject coal;
    [SerializeField] private GameObject furnaceLight;
    private HDAdditionalLightData lightData;
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;
    void Start()
    {
        coal.SetActive(false);
        ci = gameObject.GetComponent<CoalInteraction>();
        lightData = furnaceLight.GetComponent<HDAdditionalLightData>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (coalFed > 3 && !shovelMoving)
        {
            ci.Done = true;
            coalFed = 0;
        }
    }

    void Update()
    {
        if (ci.hasShovel && Input.GetKeyDown(KeyCode.Mouse0) && !shovelMoving)
        {
            // move the fucking shovel along z.axis
            StartCoroutine(MoveShovel());
        }
    }

    private IEnumerator MoveShovel()
    {
        float startTime = Time.time;
        float ogZpos = gameObject.transform.localPosition.z;
        float ogYpos = gameObject.transform.localPosition.y;
        bool hadCoalAtStart = hasCoal;
        while (Time.time - startTime <= 1)
        {
            shovelMoving = true;
            if (!hadCoalAtStart)
            {
                gameObject.transform.localPosition = new Vector3(
                    gameObject.transform.localPosition.x,
                    ogYpos - 0.5f * Mathf.Sin(Mathf.PI * (Time.time - startTime)),
                    ogZpos + Mathf.Sin(Mathf.PI * (Time.time - startTime))
                );
            }
            else
            {
                gameObject.transform.localPosition = new Vector3(
                    gameObject.transform.localPosition.x,
                    gameObject.transform.localPosition.y,
                    ogZpos + Mathf.Sin(Mathf.PI * (Time.time - startTime))
                );
            }
            yield return null;
        }
        shovelMoving = false;
        yield return null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Coal") && shovelMoving)
        {
            if (hasCoal) return;
            coal.SetActive(true);
            hasCoal = true;
            audioSource.clip = clips[0];
            audioSource.Play();
        }
        else if (other.CompareTag("Furnace") && shovelMoving)
        {
            if (!hasCoal) return;
            coal.SetActive(false);
            hasCoal = false;
            Debug.Log("put coal into furnace");
            coalFed++;
            audioSource.clip = clips[1];
            audioSource.Play();
            // Bump up the light for a bit
            StartCoroutine(Light());
        }
    }

    private IEnumerator Light()
    {
        float startTime = Time.time;
        while (Time.time - startTime <= 1f)
        {
            lightData.intensity += Mathf.Sin(2 * Mathf.PI * (Time.time - startTime));
            yield return null;
        }
    }

}
