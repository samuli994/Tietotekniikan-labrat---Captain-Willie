using UnityEngine;

public class SteeringWatcher : MonoBehaviour
{
    public float requiredWatchTime = 2f;
    public float angleThreshold = 30f; // Adjustable angle threshold

    private bool isWatching = false;
    private float watchTimer = 0f;
    private Vector3 lastLookDirection;

    public SteeringInteract steeringInteract;

    private void Update()
    {
        CheckWatchedObject();
    }

    private void CheckWatchedObject()
    {
        // Get the main camera
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
            return;
        }

        // Calculate the direction from the camera to the object
        Vector3 toObjectDirection = (transform.position - mainCamera.transform.position).normalized;

        // Check if the player is looking at the object with the same angle
        bool isLookingAtObject = Vector3.Angle(mainCamera.transform.forward, toObjectDirection) <= angleThreshold;

        // Update watching status
        if (isLookingAtObject)
        {
            if (!isWatching)
            {
                // Player just started watching
                isWatching = true;
                lastLookDirection = toObjectDirection;
                watchTimer = 0f;
            }
            else
            {
                // Player is still watching, check time
                watchTimer += Time.deltaTime;

                if (watchTimer >= requiredWatchTime)
                {
                    steeringInteract.SpawnWillie();
                }
            }
        }
        else
        {
            // Player stopped watching or changed the viewing angle
            isWatching = false;
            watchTimer = 0f;
        }
    }
}
