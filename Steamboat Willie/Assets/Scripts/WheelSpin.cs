using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSpin : MonoBehaviour
{
    public float rotationSpeed = 10f; // Adjust the speed as needed

    void FixedUpdate()
    {
        // Rotate the wheel on its Z-axis using Time.deltaTime
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
