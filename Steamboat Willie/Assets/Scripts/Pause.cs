using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{

    private bool paused;
    private FirstPersonController fpc;
    private Slider volumeSlider;
    [SerializeField] GameObject pauseContent;
    [SerializeField] Image resumeButtonImage;
    [SerializeField] Sprite buttonSprite;

    // Start is called before the first frame update
    void Start()
    {
        fpc = FindAnyObjectByType<FirstPersonController>();
        volumeSlider = GameObject.Find("VolumeSlider/Slider").GetComponent<Slider>();
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 1f);
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 1f);
        pauseContent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // I dont fucking know why
        if (paused)
        {
            resumeButtonImage.sprite = buttonSprite;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            Cursor.lockState = CursorLockMode.None;
            paused = true;
            Time.timeScale = 0f;
            pauseContent.SetActive(true);
            fpc.cameraCanMove = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            Resume();
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        PlayerPrefs.Save();
        pauseContent.SetActive(false);
        fpc.cameraCanMove = true;
        paused = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnVolumeChanged()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        Debug.Log($"set volume to: {volumeSlider.value}");
        AudioListener.volume = PlayerPrefs.GetFloat("volume", 1f);
        PlayerPrefs.Save();
    }
}
