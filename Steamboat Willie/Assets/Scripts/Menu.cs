using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    private Slider volumeSlider;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        // on start set the volume slider to what it was in playerprefs or 100
        volumeSlider = GameObject.Find("VolumeSlider/Slider").GetComponent<Slider>();
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 1f);
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 1f);
    }

    public void Play()
    {
        Debug.Log("Loading GameplayScene");
        SceneManager.LoadScene("GameplayScene");
    }

    public void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void OnVolumeChanged()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        Debug.Log($"set volume to: {volumeSlider.value}");
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 1f);
    }
}
