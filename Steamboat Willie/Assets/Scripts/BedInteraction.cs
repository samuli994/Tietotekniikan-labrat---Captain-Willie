using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BedInteraction : Interactable
{

    private MopManager mopManager;
    private PotatoInteract pi;
    public Fade fader;

    public Light nightlight;
    private float lightIntensity;

    public FirstPersonController fpc;
    public DateFade dateFader;

    public AudioSource audioSource;

    public AudioSource knockKnock;
    public AudioSource apeShit;
    public AudioSource doorsBeingOpened;

    public GameObject lightToHide;
    public GameObject lightOnHand;

    public HatchInteract hatchInteract;

    public GameObject bedLight;
    public GameObject bedLightCandle;

    public GameObject Willie;
    public GameObject HungryWillie;
    public GameObject AIWillie;
    private DoorManager dm;
    private Vector3 sleepingPos = new Vector3(4.26599979f, 2.94400001f, 6.7420001f);
    private Vector3 originalPos = new Vector3(3.49699998f, 3.95000005f, 7.80700016f);
    private Quaternion sleepingRot = new Quaternion(0f, -0.481109351f, 0f, 0.876660645f);
    private Quaternion originalRot = new Quaternion(0f, 0f, 0f, 1f);
    private bool sleeping = false;
    public SteeringWatcher steeringWatcher;

    public AudioSource bgMusic;
    public AudioClip audioClip;
    public AudioClip cursedClip;
    public SteeringInteract steeringInteract;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mopManager = FindAnyObjectByType<MopManager>();
        pi = FindAnyObjectByType<PotatoInteract>();
        lightIntensity = nightlight.intensity;
        dm = FindAnyObjectByType<DoorManager>();
        bgMusic = FindAnyObjectByType<FindMeMusic>().GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.sleepTask)
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
        if (GameManager.Instance.currentDay == 10)
        {
            bedLight.SetActive(true);
            lightOnHand.SetActive(false);
        }

        if (canInteract && !sleeping)
        {
            StartCoroutine(DayAndNight());
        }
    }

    private IEnumerator DayAndNight()
    {
        steeringInteract.steeringDone = false;
        sleeping = true;
        fpc.playerCanMove = false;
        fpc.enableHeadBob = false;
        fpc.enableSprint = false;
        fpc.enableJump = false;
        fpc.transform.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        fpc.transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        float slerpDuration = 1.5f;
        float slerpStartTime = Time.time;
        Vector3 slerpStartPos = fpc.transform.position;
        Quaternion slerpStartRot = fpc.transform.rotation;
        while (Time.time - slerpStartTime < slerpDuration)
        {
            fpc.transform.position = Vector3.Slerp(slerpStartPos, sleepingPos, (Time.time - slerpStartTime) / slerpDuration);
            fpc.transform.rotation = Quaternion.Slerp(slerpStartRot, sleepingRot, (Time.time - slerpStartTime) / slerpDuration);
            yield return null;
        }


        lightToHide.SetActive(true);
        fader.FadeIn(5);
        yield return new WaitForSeconds(3f);
        audioSource.Play();
        yield return new WaitForSeconds(2f);

        switch (GameManager.Instance.currentDay)
        {
            case 1:
                dateFader.gameObject.GetComponent<TMP_Text>().SetText("29.04.1929 Day 2");
                break;
            case 2:
                dateFader.gameObject.GetComponent<TMP_Text>().SetText("30.04.1929 Day 3");
                break;
            case 10:
                dateFader.gameObject.GetComponent<TMP_Text>().SetText("01.05.1929 Day 4");
                break;
            case 4:
                dateFader.gameObject.GetComponent<TMP_Text>().SetText("05.05.1929 Day 9");
                break;
            case 5:
                dateFader.gameObject.GetComponent<TMP_Text>().SetText("14.05.1929 Day 18");
                break;
            case 6:
                dateFader.gameObject.GetComponent<TMP_Text>().SetText("15.05.1929 Day 19");
                break;
            case 7:
                dateFader.gameObject.GetComponent<TMP_Text>().SetText("16.05.1929 Day 20");
                break;
            default:
                Debug.Log("Date not found");
                break;
        }

        if (GameManager.Instance.currentDay != 3)
        {
            dateFader.FadeIn();
        }

        yield return new WaitForSeconds(4f);
        if (GameManager.Instance.currentDay == 5)
        {
            Willie.transform.localPosition = new Vector3(4.39f, -0.831f, -0.179f);
            Willie.transform.localRotation = new Quaternion(-0.00510004768f, 0.902879119f, 0.00242811721f, 0.429857552f);
            nightlight.intensity = 0.035f;
            bedLightCandle.SetActive(true);
            bedLight.SetActive(true);
        }
        if (GameManager.Instance.currentDay == 3)
        {
            Willie.transform.localPosition = new Vector3(3.66928363f, -15.684f, -3.69099998f);
            Willie.transform.localRotation = new Quaternion(0f, -0.151956946f, 0f, -0.988387108f);
            nightlight.intensity = 0.035f;
            bedLight.SetActive(true);
        }

        if (GameManager.Instance.currentDay == 10)
        {
            lightToHide.SetActive(true);
            Willie.transform.localPosition = new Vector3(3.66928363f, 3.30220389f, -20.8951874f);
            Willie.transform.localRotation = new Quaternion(0, 0.999532759f, 0, 0.0305666793f);
        }

        if (GameManager.Instance.currentDay == 7)
        {
            HungryWillie.SetActive(false);
            Willie.transform.localPosition = new Vector3(3.66928363f, -15.684f, -3.69099998f);
            Willie.transform.localRotation = new Quaternion(0f, -0.151956946f, 0f, -0.988387108f);
            nightlight.intensity = 0.035f;
            AIWillie.SetActive(true);
            bedLight.SetActive(true);
            GameManager.Instance.sleepTask = false;
        }

        if (GameManager.Instance.currentDay != 3 && GameManager.Instance.currentDay != 5 && GameManager.Instance.currentDay != 7)
        {
            nightlight.intensity = lightIntensity;
            bedLight.SetActive(false);
        }

        dm.CloseDoors();
        yield return new WaitForSeconds(1f);
        fader.FadeOut(5);
        dateFader.FadeOut();
        if (GameManager.Instance.currentDay == 3)
        {
            doorsBeingOpened.Play();
            yield return new WaitForSeconds(20f);
        }
        if (GameManager.Instance.currentDay == 5)
        {
            knockKnock.Play();
        }
        if (GameManager.Instance.currentDay == 6)
        {
            bgMusic.clip = cursedClip;
            bgMusic.Play();
            steeringWatcher.enabled = false;
            Willie.SetActive(false);
            HungryWillie.SetActive(true);
        }
        if (GameManager.Instance.currentDay == 7)
        {
            apeShit.Play();
            yield return new WaitForSeconds(18f);
        }

        // This is kinda wrong as the slerp should be between start pos and original pos, not with current pos
        slerpStartTime = Time.time;
        while (Time.time - slerpStartTime < slerpDuration)
        {
            fpc.transform.position = Vector3.Slerp(fpc.transform.position, originalPos, (Time.time - slerpStartTime) / slerpDuration);
            fpc.transform.rotation = Quaternion.Slerp(fpc.transform.rotation, originalRot, (Time.time - slerpStartTime) / slerpDuration);
            yield return null;
        }

        fpc.transform.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        fpc.transform.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        fpc.enableSprint = true;
        fpc.enableJump = true;
        fpc.playerCanMove = true;
        fpc.enableHeadBob = true;
        hatchInteract.anim.SetBool("Open", false);
        hatchInteract.open = false;
        //TODO DATE SHOWS UP AND MAYBE SOME SOUNDS
        GameManager.Instance.TaskDone();
        mopManager.ActivateGameObjects();
        pi.DestroyChoppedPotatoes();
        sleeping = false;
    }
}
