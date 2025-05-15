using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class DirtInteract : Interactable
{
    public AudioSource brushing;
    public Animator anim;
    public DecalProjector decalProjector;

    public float ogTransparency;
    public float fadeDuration = 1f; // Set the duration of the fade

    void Start()
    {
        decalProjector = gameObject.transform.parent.GetComponent<DecalProjector>();
        ogTransparency = decalProjector.fadeFactor;

    }

    void FixedUpdate()
    {
        if (cleaning)
        {
            canInteract = true;
        }
        else 
        {
            canInteract = false;
        }
    }

    public override void Interact()
    {
        // TODO: ALPHA COROUTINE 
        if (cleaning)
        {
            anim = GameObject.FindGameObjectWithTag("Brush").GetComponent<Animator>();
            StartCoroutine(BrushAndDisable());
        }
    }

    public void BringBackTransparency()
    {
        decalProjector.fadeFactor = ogTransparency;
    }

    IEnumerator BrushAndDisable()
    {
        brushing.Play();
        anim.SetBool("Brush", true);

        // Get the initial transparency value
        float initialTransparency = decalProjector.fadeFactor;

        // Calculate the transparency step based on the fade duration
        float transparencyStep = initialTransparency / fadeDuration;

        // Gradually decrease transparency to 0
        while (decalProjector.fadeFactor > 0)
        {
            float currentAlpha = decalProjector.fadeFactor;
            currentAlpha -= transparencyStep * Time.deltaTime;
            decalProjector.fadeFactor = currentAlpha;
            yield return null;
        }
        anim.SetBool("Brush", false);

        // Disable the parent object after the brushing sound has finished playing and the fade is complete
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
