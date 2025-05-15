using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderTheDeckTrigger : MonoBehaviour
{
    public Light light;
    private float intensity;
    public GameObject willie;
    // Start is called before the first frame update

    void Start()
    {
        intensity = light.intensity;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            light.intensity = 0;
            GameManager.Instance.underTheDeck = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Instance.underTheDeck = false;
            if (GameManager.Instance.currentDay == 10 || GameManager.Instance.currentDay == 8)
            {
                light.intensity = 0.035f;
                if (GameManager.Instance.currentDay == 8)
                {
                    willie.SetActive(false);
                }
            }
            else
            {
                light.intensity = intensity;
            }
        }
    }
}
