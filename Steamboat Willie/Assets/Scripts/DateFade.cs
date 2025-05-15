using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DateFade : MonoBehaviour
{
    public float fadeTime;
    public bool autoFadeOutAtStart = false;
    private TMP_Text text;
    private bool isFading;
    private bool isFadingIn;

    void Start()
    {
        text = GetComponent<TMP_Text>();
        isFading = false;
        isFadingIn = false;
        if (autoFadeOutAtStart)
        {
            FadeOut();
        }
    }

    public void FadeOut()
    {
        if (!isFading)
        {
            isFading = true;
            StartCoroutine(FadeOutRoutine());
        }
    }

    public void FadeIn()
    {
        if (!isFadingIn)
        {
            isFadingIn = true;
            StartCoroutine(FadeInRoutine());
        }
    }

    IEnumerator FadeOutRoutine()
    {
        while (text.color.a > 0)
        {
            Color tempColor = text.color;
            tempColor.a -= Time.deltaTime / fadeTime;
            text.color = tempColor;
            yield return null;
        }
        isFading = false;
    }

    IEnumerator FadeInRoutine()
    {
        while (text.color.a < 1)
        {
            Color tempColor = text.color;
            tempColor.a += Time.deltaTime / fadeTime;
            text.color = tempColor;
            yield return null;
        }
        isFadingIn = false;
    }
}