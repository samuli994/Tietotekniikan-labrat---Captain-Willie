using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSway : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Sway());
    }

    private IEnumerator Sway()
    {
        float startTime = Time.time;
        while (true)
        {
            float currTime = Time.time - startTime;
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Sin(currTime) / 2);
            yield return new WaitForSeconds(0.0001f);
        }
    }
}
