using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitScene : MonoBehaviour
{
    public Text debugText;
    public Button serverButton;
    public ProgressBar progress;


    // Start is called before the first frame update
    void Start()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-server")
            {
                Server();
                break;
            }
            else if (args[i] == "-client")
            {
                Client();
                break;
            }
        }
        StartCoroutine(Detect());
    }
    float p = 0;

    public IEnumerator Detect()
    {
        p = Time.time;

        if (Debug.isDebugBuild)
        {
            debugText.text = "Debug Mode";
            progress.currentPercent = 0f;
            serverButton.interactable = true;
        }

        while (p+30 > Time.time)
        {
            yield return 0;
            progress.currentPercent = (Time.time % 1f) * 100f;
        }
        if (serverButton.interactable)
        {
            debugText.text = "Server Found";
            progress.currentPercent = 99.99f;
        }
        else
        {
            debugText.text = "MM Server Not Found";
            progress.currentPercent = 0f;
        }
    }


    public void Client()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
    public void Server()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Server");
    }
}
