using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneLeverInteract : Interactable
{

    [SerializeField] private GameObject boat;
    [SerializeField] private GameObject _lock;
    [SerializeField] private GameObject lever;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioClip unlockFail;
    [SerializeField] private AudioClip unlockSuccess;
    private AudioSource source;
    private Quaternion leverRot = new Quaternion(-0.400748193f, -0.582581282f, 0.582581341f, 0.400748074f);

    public GameObject SpawnChaserWillie;


    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        source.loop = false;
    }

    private void FixedUpdate()
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
            if (GameManager.Instance.hasKey)
            {
                // drop the boat
                StartCoroutine(BoatDrop());
                SpawnChaserWillie.SetActive(true);
            }
            else
            {
                // fuck w/ the lock
                anim.ResetTrigger("Wiggle");
                anim.SetTrigger("Wiggle");
                source.clip = unlockFail;
                source.Play();
            }
        }
    }

    private IEnumerator BoatDrop()
    {
        source.clip = unlockSuccess;
        source.Play();
        _lock.SetActive(false);
        yield return new WaitForSeconds(1f);
        float startTime = Time.time;
        GetComponent<AudioSource>().Play();
        Quaternion startRot = lever.transform.rotation;
        while (Time.time - startTime < 2)
        {
            lever.transform.rotation = Quaternion.Lerp(startRot, leverRot, (Time.time - startTime) / 2);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        boat.GetComponent<Rigidbody>().useGravity = true;
    }

}
