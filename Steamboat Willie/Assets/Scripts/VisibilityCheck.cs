using System.Collections;
using UnityEngine;

public class VisibilityCheck : MonoBehaviour
{
    private bool isVisible = false;
    private float visibilityTimer = 0f;
    public float requiredVisibilityTime = 1f;

    public float visibilityAngle = 45f; // Adjustable angle threshold

    public DoorInteract doorInteract;

    private void Update()
    {
        CheckVisibility();
    }

    private void Noticed() {
        GetComponent<AudioSource>().Play();
        GetComponent<Animator>().SetBool("Peek",false);
        StartCoroutine(ShutTheDoor());

    }

    IEnumerator ShutTheDoor() {
        Debug.Log("Timer started");
        yield return new WaitForSeconds(1.2f);
        doorInteract.ShutTheDoor();
        gameObject.SetActive(false);
    }

    private void CheckVisibility()
    {
        // Get the main camera
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        // Calculate the viewport position of the object
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);

        // Check if the object is within the camera's view frustum and angle threshold
        bool isObjectVisible = (viewportPosition.x >= 0 && viewportPosition.x <= 1) &&
                               (viewportPosition.y >= 0 && viewportPosition.y <= 1) &&
                               (viewportPosition.z > 0) &&
                               (Vector3.Angle(mainCamera.transform.forward, transform.position - mainCamera.transform.position) <= visibilityAngle);


        // Update visibility status
        if (isObjectVisible)
        {
            visibilityTimer += Time.deltaTime;

            if (visibilityTimer >= requiredVisibilityTime && !isVisible)
            {
                // Object has been visible for 1 second
                isVisible = true;
                Noticed();
            }
        }
        else
        {
            isVisible = false;
            visibilityTimer = 0f;
        }
    }
}