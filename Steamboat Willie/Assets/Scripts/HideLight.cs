using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLight : MonoBehaviour
{
    public GameObject lightToHide;
    private bool showLight = true;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.currentDay == 10 && GameManager.Instance.currentDay == 8) {
            lightToHide.SetActive(false);
        }
        if (GameManager.Instance.currentDay == 4) {
            if (showLight)
            {
                lightToHide.SetActive(true);
                showLight = false;
            }
        }
    }
}
