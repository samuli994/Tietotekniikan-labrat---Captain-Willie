using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringInteract : Interactable
{
    private Vector3 steeringPos = new Vector3(0.358358294f, 8.07525635f, -10.5369034f);
    private FirstPersonController fpc;
    private GameObject player;

    public GameObject Willie;

    public bool steeringDone = false;
    public SteeringWatcher steeringWatcher;
    public SpeechManager speechManager;

    public bool driving = true;
    private bool isDriving = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fpc = player.GetComponent<FirstPersonController>();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.driveTask)
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
        if (canInteract && !isDriving)
        {
            isDriving = true;
            // Hide willie? he walks away??
            GetComponent<AudioSource>().Play();
            driving = true;
            Willie.SetActive(false);
            fpc.enableSprint = false;
            fpc.playerCanMove = false;
            fpc.enableHeadBob = false;
            fpc.enableJump = false;
            StartCoroutine(SlerpToPos());
        }
    }

    private IEnumerator SlerpToPos()
    {
        float startTime = Time.time;
        float duration = 1f;
        while (Time.time - startTime <= duration)
        {
            player.transform.position = Vector3.Slerp(player.transform.position, steeringPos, (Time.time - startTime) / duration);
            yield return null;
        }
        StartCoroutine(Steering());
    }

    private IEnumerator Steering()
    {
        float startTime = Time.time;
        float duration = 30f;
        while (driving)
        {
            // steering
            if (Input.GetKey(KeyCode.A))
            {
                // move the wheel left
                gameObject.transform.Rotate(0f, 0f, -100f * Time.deltaTime);
                yield return null;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                // move right
                gameObject.transform.Rotate(0f, 0f, 100f * Time.deltaTime);
                yield return null;
            }
            else
            {
                if (gameObject.transform.rotation.z < -0.01)
                {
                    gameObject.transform.Rotate(0f, 0f, 100f * Time.deltaTime);
                }
                else if (gameObject.transform.rotation.z > 0.01)
                {
                    gameObject.transform.Rotate(0f, 0f, -100f * Time.deltaTime);
                }
            }
            yield return null;
            if (Time.time - startTime > duration)
            {
                steeringWatcher.enabled = true;
                steeringDone = true;
            }

        }
        isDriving = false;

    }

    public void SpawnWillie()
    {
        if (steeringDone)
        {
            Willie.SetActive(true);
            Willie.transform.localPosition = new Vector3(3.66928363f, 3.30220389f, -19.1949997f);
            Willie.transform.localRotation = new Quaternion(0f, 0.999532759f, 0f, 0.0305666793f);
            speechManager.HideText();
            steeringWatcher.enabled = false;
        }
    }
}
