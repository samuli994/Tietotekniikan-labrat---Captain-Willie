using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public float fadeTime;
    public bool autoFadeOutAtStart = false;
    private Image image;
    private bool isFading;
    private bool isFadingIn;

    void Start()
    {
        image = GetComponent<Image>();
        isFading = false;
        isFadingIn = false;
        if (autoFadeOutAtStart)
        {
            FadeOut(fadeTime);
        }
    }

    public void FadeOut(float fadetime)
    {
        if (!isFading)
        {
            isFading = true;
            StartCoroutine(FadeOutRoutine(fadetime));
        }
    }

    public void FadeIn(float fadetime)
    {
        if (!isFadingIn)
        {
            isFadingIn = true;
            StartCoroutine(FadeInRoutine(fadetime));
        }
    }

    IEnumerator FadeOutRoutine(float fadeTime)
    {
        while (image.color.a > 0)
        {
            Color tempColor = image.color;
            tempColor.a -= Time.deltaTime / fadeTime;
            image.color = tempColor;
            yield return null;
        }
        isFading = false;
    }

    IEnumerator FadeInRoutine(float fadeTime)
    {
        while (image.color.a < 1)
        {
            Color tempColor = image.color;
            tempColor.a += Time.deltaTime / fadeTime;
            image.color = tempColor;
            yield return null;
        }
        isFadingIn = false;
    }

}