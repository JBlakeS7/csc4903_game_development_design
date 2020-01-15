using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject Pause;
    public GameObject Options;
    public GameObject Controls;
    public Slider volumeSlider;
    public Slider mouseSlider;


    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        mouseSlider.value = PlayerPrefs.GetFloat("Mouse", 5f);
        AudioListener.volume = volumeSlider.value;
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && FPSControllerLPFP.FpsControllerLPFP.player)
        {
            if (Options.activeSelf == true)
            {
                Options.SetActive(false);
                Pause.SetActive(true);
                PlayerPrefs.SetFloat("Mouse", mouseSlider.value);
                PlayerPrefs.SetFloat("Volume", volumeSlider.value);
            }
            else if(Controls.activeSelf == true)
            {
                Controls.SetActive(false);
                Pause.SetActive(true);
            }
            else
            {
                if(Pause.activeSelf)
                {
                    Resume();
                }
                else
                {
                    Pause.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    paused = true;
                }
            }

        }
        if (Options.activeSelf)
        {
            AudioListener.volume = volumeSlider.value;
        }
    }


    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        paused = false;
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetFloat("Mouse", mouseSlider.value);
    }

    public void Resume()
    {
        paused = false;
        Pause.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetFloat("Mouse", mouseSlider.value);
    }

    public void Exit()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetFloat("Mouse", mouseSlider.value);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
        Destroy(this.gameObject);
    }
}
