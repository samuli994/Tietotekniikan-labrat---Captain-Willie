using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    [SerializeField] private float hitDetectionDistance = 3f;
    [SerializeField] private Sprite normalCrosshair;
    [SerializeField] private Sprite interactCrosshair;

    [SerializeField] private Image crosshair;
    private RectTransform crosshairTransform;
    private Vector3 normalScale = new Vector3(0.02f, 0.02f, 0.02f);
    private Vector3 interactScale = new Vector3(0.3f, 0.3f, 0.3f);


    void Start()
    {
        crosshairTransform = crosshair.GetComponent<RectTransform>();
    }


    // Update is called once per frame
    void Update()
    {
        crosshair.sprite = normalCrosshair;
        crosshairTransform.localScale = normalScale;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, hitDetectionDistance, -5, QueryTriggerInteraction.Ignore))
        {
            GameObject target = hit.transform.gameObject;
            if (target.CompareTag("Interactable"))
            {
                if (target.GetComponent<Interactable>().canInteract)
                {
                    crosshair.sprite = interactCrosshair;
                    crosshairTransform.localScale = interactScale;
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();
                    interactable.Interact();
                }
            }
        }
    }
}
