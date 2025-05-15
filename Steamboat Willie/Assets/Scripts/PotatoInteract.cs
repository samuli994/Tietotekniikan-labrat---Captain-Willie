using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PotatoInteract : Interactable
{
    private FirstPersonController fpc;
    private GameObject cam;
    private GameObject player;
    private Vector3 peelingPos = new Vector3(-2.26644969f, 3.76448989f, 10.1719666f);
    private Quaternion peelingRot = new Quaternion(0f, 0.910689473f, 0f, -0.4130916f);
    private Quaternion peelingPitch = new Quaternion(0.088024959f, 0f, 0f, 0.996118307f);
    private GameObject swayParent;
    [SerializeField] private Transform potatoSpot;
    [SerializeField] private Transform knifeSpot;
    [SerializeField] private GameObject Potato;
    [SerializeField] private GameObject donePotato;
    [SerializeField] private GameObject jumpScare;
    [SerializeField] private GameObject wheelTaskGiver;
    [SerializeField] private AudioSource chopSound;
    private GameObject potatoPeeler8000;
    private Vector3 ogPosPeeler;
    private Quaternion ogRotPeeler;
    private Transform ogParentPeeler;
    private List<GameObject> choppedPotatos = new List<GameObject>();
    public DoorInteract doorInteract;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fpc = player.GetComponent<FirstPersonController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        swayParent = GameObject.Find("SwayParent");
        potatoPeeler8000 = GameObject.Find("PeelingKnife");
        ogPosPeeler = potatoPeeler8000.transform.localPosition;
        ogRotPeeler = potatoPeeler8000.transform.localRotation;
        ogParentPeeler = potatoPeeler8000.transform.parent;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.potatoTask)
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
        // lock player and camera in place
        //fpc.cameraCanMove = false;

        // TODO: -- Remember to reset these afterwards
        if (canInteract)
        {
            if (GameManager.Instance.GetCurrentDay() == 3)
            {
                if (doorInteract.doorOpen) {
                    jumpScare.SetActive(true);
                }
            }

            if (GameManager.Instance.GetCurrentDay() == 2)
            {
                wheelTaskGiver.transform.localPosition = new Vector3(4.51200008f, -0.84799999f, -0.870000005f);
                wheelTaskGiver.transform.localRotation = new Quaternion(0.000879898085f, 0.778693438f, -0.000779236725f, -0.627403557f);

            }
            fpc.enableSprint = false;
            fpc.playerCanMove = false;
            fpc.enableHeadBob = false;
            fpc.enableJump = false;
            StartCoroutine(SetPlayerPos());
        }
    }

    public void DestroyChoppedPotatoes()
    {
        foreach (var potato in choppedPotatos)
        {
            Destroy(potato);
        }
    }

    private IEnumerator SetPlayerPos()
    {
        // set player to position and camera
        float startTime = Time.time;
        float duration = 0.7f;
        while (Time.time - startTime <= duration)
        {
            player.transform.SetPositionAndRotation(
                Vector3.Slerp(player.transform.position, peelingPos, (Time.time - startTime) / duration),
                Quaternion.Slerp(player.transform.rotation, peelingRot, (Time.time - startTime) / duration)
                );
            yield return null;
        }

        // after player ahs been set to spot add X potatoes to peel
        StartCoroutine(PeelPotatos(4));
    }

    private IEnumerator PeelPotatos(int amount)
    {
        // set the knife in place for cutting
        potatoPeeler8000.transform.parent = knifeSpot;
        potatoPeeler8000.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        for (int i = 0; i < amount; i++)
        {
            int potatoPeelCounter = 0;
            GameObject potato = Instantiate(Potato);
            GameObject cutPotato;
            potato.transform.parent = swayParent.transform;
            potato.transform.position = potatoSpot.position;
            while (!potato.IsDestroyed())
            {
                // wait for inputs
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    potatoPeelCounter++;
                    // small anim
                    float cutStartTime = Time.time;
                    chopSound.Play();
                    while (Time.time - cutStartTime <= 0.5f)
                    {
                        potatoPeeler8000.transform.localRotation = Quaternion.Euler(
                            0f,
                            0f,
                            50f * Mathf.Sin(2 * Mathf.PI * (Time.time - cutStartTime))
                            );
                        yield return null;
                    }
                }

                if (potatoPeelCounter >= 5)
                {
                    Destroy(potato);
                    cutPotato = Instantiate(donePotato);
                    cutPotato.transform.parent = swayParent.transform;
                    cutPotato.transform.position = potatoSpot.transform.position;
                    choppedPotatos.Add(cutPotato);

                }
                yield return null;
            }
            yield return new WaitForSeconds(1f);
        }
        yield return null;
        fpc.enableSprint = true;
        fpc.playerCanMove = true;
        fpc.enableHeadBob = true;
        fpc.enableJump = true;

        // set the knife back
        potatoPeeler8000.transform.parent = ogParentPeeler;
        potatoPeeler8000.transform.SetLocalPositionAndRotation(ogPosPeeler, ogRotPeeler);
        GameManager.Instance.TaskDone();
    }


}
