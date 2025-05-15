using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using Newtonsoft.Json.Linq;
using Microsoft.CSharp;

public class SpeechManager : MonoBehaviour
{
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private TMP_Text textObj;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        speechBubble.SetActive(false);
    }

    public void ShowText(int day, string task)
    {
        time = Time.time;
        speechBubble.SetActive(true);
        string text = GetText(day, task);
        textObj.SetText(text);
        StartCoroutine(HideTextAfter(10f, time));
    }

    private IEnumerator HideTextAfter(float duration, float time)
    {
        yield return new WaitForSeconds(duration);
        if (time == this.time)
        {
            HideText();
        }
    }

    public void HideText()
    {
        speechBubble.SetActive(false);
    }

    private string GetText(int day, string task)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "WillieSpeech.json");

        if (File.Exists(filePath))
        {
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                dynamic res = JObject.Parse(json);
                return (string)res["day" + day][task];
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
            return null;
        }
    }
}
