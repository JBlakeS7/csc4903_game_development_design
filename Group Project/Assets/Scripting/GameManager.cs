using BIAB.Networking;
using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages Game State
/// Works with NetworkManager To Handle Global Behavior
/// 
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager main;
    private void Awake()
    {
        if (main == null)
            main = this;
        else
        {
            Debug.Log("More Than One GameManager Attempted Registration!");
            Destroy(this);
        }
    }
    string[] maps = new string[] { "Colossium" };
    

    private bool _isSinglePlayer = true;
    public bool isSinglePlayer()
    {
        return _isSinglePlayer;
    }

    public GameMode loadedGameMode;
    public GameObject PlayerPrefab;
    public CanvasGroup Blackout;
    public CanvasGroup Loading;
    public ProgressBar LoadingBar;

    private void Start()
    {
        Init();
    }


    /// <summary>
    /// This happens when entering playmode
    /// </summary>
    public void Init()
    {
        _isSinglePlayer = true;
        GoToPlayMode();
    }

    /// <summary>
    /// Used to Change The Client into New Network Operations Mode
    /// </summary>
    /// <param name="isSinglePlayer"></param>
    public void SetSinglePlayer(bool isSinglePlayer)
    {
        _isSinglePlayer = isSinglePlayer;
        throw new System.NotImplementedException("Event Needs to be added to setting singleplayer in Game Manager");
    }

    public void ChangeModeAndMap(GameManagerNetMsg msg)
    {
        StartCoroutine(_ChangeModeAndMap(msg));
    }
    IEnumerator _ChangeModeAndMap(GameManagerNetMsg msg)
    {
        //Open Loading Screen

        //swtch map 
        AsyncOperation p = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(maps[msg.map]);
        LoadingBar.currentPercent = 0;
        while (!p.isDone)
        {
            LoadingBar.currentPercent = p.progress*100;
            yield return 0;
        }
        LoadingBar.currentPercent = 100;
        Destroy(loadedGameMode);
        //switch Gamemode
        switch (msg.gameMode)
        {
            case (int)GameModeNum.TDM:
                throw new System.NotImplementedException("TDM Not Registered");
                break;
            case (int)GameModeNum.CTF:
                throw new System.NotImplementedException("CTF Not Registered");
                break;
            case (int)GameModeNum.FFA:
                loadedGameMode = this.gameObject.AddComponent<FFAMode>();
                break;
            default:
                throw new System.NotImplementedException("Game Mode Not Registered with ID:"+msg.gameMode);
                break;

        }
    }

    public void GoToPlayMode()
    {
        if (Server.main != null)
            return;
        LoadingBar.currentPercent = 0;
        FadeEffect.FadeCanvasIn(Blackout);
        FadeEffect.FadeCanvasIn(Loading);
        loadedGameMode = this.gameObject.AddComponent<PracticeMode>();
        StartCoroutine(_GoToPlayMode());
    }
    IEnumerator _GoToPlayMode()
    {
        AsyncOperation k = SceneManager.LoadSceneAsync("Practice Map");
        while (k.isDone == false)
        {
            LoadingBar.currentPercent = k.progress*100;
            yield return 0;
        }
        yield return 0;
        LoadingBar.currentPercent = 100;
        StartMatch();
    }

    void StartMatch()
    {
        loadedGameMode.StartMatch();
        FadeEffect.FadeCanvasOut(Blackout);
        FadeEffect.FadeCanvasOut(Loading);
    }
    //IsServerLoop
    //Tell Clients GamemOde and Map
    //Load Map
    //Load GameMode
    //Wait until all have loaded
    //GameMode.StartMatch();

    // Load Map (int)
    // Load GameMode (int)
    // Pause
    // Resume

}

public enum GameModeNum
{
    FFA,
    TDM,
    CTF
}