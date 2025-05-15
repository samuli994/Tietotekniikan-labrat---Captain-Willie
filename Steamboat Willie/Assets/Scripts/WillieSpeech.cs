using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillieSpeech : MonoBehaviour
{
    public GameObject fpc;
    public float offSet;

    public bool offset = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion originalRotation = gameObject.transform.rotation;

        gameObject.transform.LookAt(fpc.transform);
        Quaternion newRotation = gameObject.transform.rotation;

        // Apply a Y-axis offset of -180 degrees
        if (offset) {
            newRotation *= Quaternion.Euler(0, 0, 0);
        }
        else {
            newRotation *= Quaternion.Euler(-280, 0, 0);
        }

        // Preserve the original rotations on X and Z axes
        if (offset) {
            newRotation.x = originalRotation.x;
            newRotation.z = originalRotation.z;
        }
        

        gameObject.transform.rotation = newRotation;
    }
}
