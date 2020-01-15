using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider mouseSlider;
    public UnityEngine.UI.Text graphicSetting;

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        mouseSlider.value = PlayerPrefs.GetFloat("Mouse", 5f);
        graphicSetting.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game Setup");
    }
    public void Horde()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Horde Setup");
    }
    public void Bootcamp()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Tutorial");
    }

    public void MusicTest()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Music Test");
    }
    public void NetworkingTest()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Networking Test");
    }

    private void Update()
    {
        AudioListener.volume = volumeSlider.value;
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("Mouse", mouseSlider.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }

    public void IncreaseGraphics()
    {
        QualitySettings.IncreaseLevel();
        graphicSetting.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void DecreaseGraphics()
    {
        QualitySettings.DecreaseLevel();
        graphicSetting.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }


    public void Exit()
    {
        Application.Quit();
        Debug.Break();
    }

}
