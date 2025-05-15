using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{

    public Animator anim;
    private bool startCoroutine = true;
    private FirstPersonController fpc;
    public GameObject endingCamera;
    public GameObject endingCamera2;
    public GameObject cube;
    [SerializeField] private GameObject thanksText;
    [SerializeField] private Fade fader;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; // if something else than the player touches trigger do nothing
        fpc = other.GetComponent<FirstPersonController>();
        if (startCoroutine)
        {
            StartCoroutine(EndSequence());
        }

        Debug.Log("Starting end sequence");
    }


    private IEnumerator EndSequence()
    {
        fpc.transform.parent = gameObject.transform;
        fpc.GetComponent<Rigidbody>().isKinematic = true;
        fpc.gameObject.transform.localPosition = new Vector3(-0.0631000027f, 0.0133999996f, 0.0472999997f);
        fpc.gameObject.transform.localRotation = new Quaternion(0.497671545f, 0.502180576f, 0.502235889f, 0.497892439f);
        fpc.gameObject.transform.localScale = new Vector3(0.100927964f, 0.163763911f, 0.0494016148f);
        endingCamera.SetActive(true);
        cube.SetActive(false);
        fpc.playerCanMove = false;
        fpc.cameraCanMove = false;
        anim.enabled = true;
        startCoroutine = false;
        anim.SetBool("Boat", true);
        yield return new WaitForSeconds(20f);
        // show text
        thanksText.SetActive(true);
        // bring text up

        float startTime = Time.time;
        RectTransform pos = thanksText.GetComponent<RectTransform>();
        float ogPos = thanksText.transform.localPosition.z;

        while (Time.time - startTime <= 3)
        {
            thanksText.transform.localPosition = new Vector3(
                thanksText.transform.localPosition.x,
                thanksText.transform.localPosition.y,
                Mathf.Lerp(ogPos, 0.02f, (Time.time - startTime) / 3)
                );
            yield return null;
        }
        yield return new WaitForSeconds(12f);
        fader.FadeIn(2f);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("Menu");
        //JOTAIN
    }
}
