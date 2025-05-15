using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlane : MonoBehaviour
{
    [SerializeField] private Fade fade;
    [SerializeField] private GameObject underTheDeck;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        underTheDeck.SetActive(false);
        fade.FadeIn(0.5f);
        if (GameManager.Instance.currentDay != 8)
        {
            StartCoroutine(ReturnToMenu());
        }
        else
        {
            GameManager.Instance.day8reset = true;
        }
    }

    private IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Menu");
        yield return null;
    }
}
