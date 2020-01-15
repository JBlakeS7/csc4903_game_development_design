using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider volumeSlider;
    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
    }
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game Setup");
    }

    private void Update()
    {
        AudioListener.volume = volumeSlider.value;
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }


    public void Exit()
    {
        Application.Quit();
        Debug.Break();
    }
}
