using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalInteraction : Interactable
{
    private GameObject shovelObj;
    private Shovel shovel;
    private Vector3 shovelOriginalPos;
    private Quaternion shovelOriginalRot;
    private Vector3 shovelOriginalScale;
    private Transform pickupParent;
    private Transform ogParent;
    private Vector3 inHandPos = new Vector3(0.583999991f, -0.405999988f, 0.419999987f);
    private Quaternion inHandRot = new Quaternion(-0.704552412f, 0.520441294f, 0.233076185f, 0.422400534f);
    public bool hasShovel = false;
    public SteeringWatcher steeringWatcher;
    public GameObject willie;

    public bool Done;


    // Start is called before the first frame update
    void Start()
    {
        shovelObj = GameObject.Find("Shovel");
        shovel = shovelObj.GetComponent<Shovel>();
        shovelOriginalPos = shovelObj.transform.localPosition;
        shovelOriginalRot = shovelObj.transform.localRotation;
        shovelOriginalScale = shovelObj.transform.localScale;

        ogParent = shovel.transform.parent;
        pickupParent = GameObject.Find("FirstPersonController/Joint").transform;
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.coalTask)
        {
            canInteract = true;
        }
        else
        {
            canInteract = false;
        }

        if (Done)
        {
            GameManager.Instance.TaskDone();
            // put shovel back
            GetComponent<AudioSource>().Play();
            shovelObj.transform.parent = ogParent;
            shovelObj.transform.SetLocalPositionAndRotation(shovelOriginalPos, shovelOriginalRot);
            shovelObj.transform.localScale = shovelOriginalScale;
            hasShovel = false;
            Done = false;
        }
    }

    public override void Interact()
    {
        if (canInteract)
        {
            
            if (hasShovel) return;
            steeringWatcher.enabled = false;
            // Pick up the showel
            shovelObj.transform.parent = pickupParent;
            hasShovel = true;
            GetComponent<AudioSource>().Play();
            willie.transform.localPosition = new Vector3(3.66928363f, 3.30220389f, -20.8951874f);
            willie.transform.localRotation = new Quaternion(0f, 0.999532759f, 0f, 0.0305666793f);
            // TODO: do something with the transforms for it to look ok
            shovelObj.transform.SetLocalPositionAndRotation(inHandPos, inHandRot);
        }
    }
}
