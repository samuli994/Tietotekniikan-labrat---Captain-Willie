using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellInteract : Interactable
{
    // Start is called before the first frame update
    private AudioSource audioSource;
    private bool isSwinging = false;
    private float swingStartTime = 0;
    private Quaternion og_rotation;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        og_rotation = gameObject.transform.rotation;
        canInteract = true;
    }

    public override void Interact()
    {
        audioSource.Play();
        isSwinging = true;
        swingStartTime = Time.time;
    }

    void Update()
    {
        if (isSwinging)
        {
            if (swingStartTime == 0) return;
            float currTime = Time.time - swingStartTime;
            transform.localRotation = Quaternion.Euler(20 * Mathf.Exp(-2 * currTime) * Mathf.Sin(2 * Mathf.PI * currTime) - 90f, -90f, 90f);

            if (currTime > 4)
            {
                transform.rotation = og_rotation;
                isSwinging = false;
                swingStartTime = 0f;
            }


        }
    }
}
