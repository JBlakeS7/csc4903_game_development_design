using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using NAudio;
using NAudio.Wave;
using System.Threading;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour {
    public GUISkin skin;
    public static Music music;
    static string CurrentSong = "None";
    string location = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic);
    bool skip = false;
    int skipSong = 0;
    int play = 0;
    bool showPL = false;
    int x = 0;
    AudioSource source;
    public MusicPlayerSettings settings;
    public Vector2 scrollPL;
    Rect windowRect = new Rect(0, 0, 250, 125);
    List<string> Paths = new List<string>();
    string timeLeft = "";
    string timeRight = "";
    bool run = true;

    public Pulse pulse;

    /*void Awake()
    {

        if (music == null)
        {
            DontDestroyOnLoad(transform.gameObject);
            music = this;
        } else
        {
            Destroy(transform.gameObject);
        }
    }*/
    // Use this for initialization
    void Start()
    {
        windowRect = new Rect(new Vector2(), Fix(40,35));
        windowRect.x = PlayerPrefs.GetInt("MusicPosX", 0);
        windowRect.y = PlayerPrefs.GetInt("MusicPosY", 0);
        source = this.GetComponent<AudioSource>();
        settings = new MusicPlayerSettings();
        
        AudioConfiguration config = AudioSettings.GetConfiguration();
        AudioSettings.Reset(config);
        string[] dir = Directory.GetFiles(location, "*.mp3", SearchOption.AllDirectories);
        foreach (string file in dir)
        {
            FileInfo info = new FileInfo(file);
            Paths.Add(info.FullName);
        }

        //StartCoroutine(Player());

        play = PlayerPrefs.GetInt("MusicPlayerAuto", 1);

        //WwW random Cycle trhough them	

    }

    void OnEnable()
    {
        if (source == null) source = this.GetComponent<AudioSource>();
        if (settings == null) settings = new MusicPlayerSettings();
        if (Paths.Count == 0)
        {
            string[] dir = Directory.GetFiles(location, "*.mp3", SearchOption.AllDirectories);
            foreach (string file in dir)
            {
                FileInfo info = new FileInfo(file);
                Paths.Add(info.FullName);
            }
        }
        windowRect.x = PlayerPrefs.GetInt("MusicPosX", 0);
        windowRect.y = PlayerPrefs.GetInt("MusicPosY", 0);
        StartCoroutine(Player());
        play = PlayerPrefs.GetInt("MusicPlayerAuto", 0);

    }

    int curr = 0;
    IEnumerator Player()
    {
        if (PlayerPrefs.GetInt("MusicPlayer", 1) == 1)
        {
            if (PlayerPrefs.GetInt("lastSong", 0) < Paths.Count)
            {
                x = PlayerPrefs.GetInt("lastSong", 0);
            }
            for (int i = 0; i < Paths.Count; i++)
            {
                if (PlayerPrefs.GetInt("MusicPlayer", 1) == 1)
                {
                    if (x != 0)
                    {
                        i = x;
                        x = 0;
                    }
                    PlayerPrefs.SetInt("lastSong", i);
                    //if (File.Exists(Application.persistentDataPath + "/temp.wav")) File.Delete(Application.persistentDataPath + "/temp.wav");
                    curr = i;
                    yield return 0;
                    FileInfo info = new FileInfo(Paths[i]);
                    if ((int)(info.Length >> 20) < PlayerPrefs.GetInt("MaxMusicSize", 30))
                    {
                        /*
                        CurrentSong = "(Reading) " + temp1;
                        Mp3FileReader reader = new Mp3FileReader(Paths[i]);
                        yield return 0;
                        CurrentSong = "(Converting) " + temp1;*/
                        //string path = Application.persistentDataPath + "/temp.wav";

                        string[] temp = Paths[i].Split('\\');
                        string temp1 = temp[temp.Length - 1].Split(new string[] { ".mp3" }, StringSplitOptions.None)[0].Trim();
                        Debug.Log(temp1 + " Size: " + string.Format("{0}Mb", info.Length >> 20));
                        CurrentSong = "(Loading) " + temp1;
                        yield return 0;
                            LoadSong(Paths[i]);
                        yield return 0;

                        while (!source.isPlaying) yield return 0;
                        CurrentSong = temp1;

                        //timeRight = timer(source.clip.length);
                        while (source.isPlaying && skip != true && x == 0 && (PlayerPrefs.GetInt("MusicPlayer", 1) == 1) && source.time < currentLength)
                            yield return 0;
                    }
                    
                    skip = false;
                    /*
                    if (i - skipSong < 0)
                    {
                        i = Paths.Count - Mathf.Abs(i - skipSong);
                    }
                    else i -= skipSong;*/
                    skipSong = 0;
                    if (i == Paths.Count - 1)
                    {
                        i = -1;
                    }

                }
            }
        }
    }

    /*
    int curr = 0;
    IEnumerator Player()
    {
        if (PlayerPrefs.GetInt("MusicPlayer", 1) == 1)
        {
            if (PlayerPrefs.GetInt("lastSong", 0) < Paths.Count)
            {
                x = PlayerPrefs.GetInt("lastSong", 0);
            }
            for (int i = 0; i < Paths.Count; i++)
            {
                if (PlayerPrefs.GetInt("MusicPlayer", 1) == 1)
                {
                    if (x != 0)
                    {
                        i = x;
                        x = 0;
                    }
                    PlayerPrefs.SetInt("lastSong", i);
                    //if (File.Exists(Application.persistentDataPath + "/temp.wav")) File.Delete(Application.persistentDataPath + "/temp.wav");
                    curr = i;
                    yield return 0;
                    FileInfo info = new FileInfo(Paths[i]);
                    if ((int)(info.Length >> 20) < PlayerPrefs.GetInt("MaxMusicSize", 250))
                    {
    string path = Application.persistentDataPath + "/temp.wav";

    string[] temp = Paths[i].Split('\\');
    string temp1 = temp[temp.Length - 1].Split(new string[] { ".mp3" }, StringSplitOptions.None)[0].Trim();
    Debug.Log(temp1 + " Size: " + string.Format("{0}Mb", info.Length >> 20));
                        Thread si = new Thread(() =>
                        {
                            run = false;
                            Thread.CurrentThread.IsBackground = true;
                            CurrentSong = "(Reading) " + temp1;
                            Mp3FileReader reader = new Mp3FileReader(Paths[i]);
                            CurrentSong = "(Converting) " + temp1;
                            File.Delete(path);
                            WaveFileWriter.CreateWaveFile(path, reader);
                            run = true;
                        });
    si.Start();
                        while (si.IsAlive) yield return new WaitForSeconds(1f);

    yield return 0;
                        CurrentSong = "(Loading) " + temp1;
                        yield return 0;
                        WWW www = new WWW("file:///" + path);
                        while (!www.isDone)
                        {
                            yield return 0;
                        }
                        if (source.clip != null)
                        {
                            source.clip.UnloadAudioData();
                            Destroy(source.clip);
                        }
                        source.clip = www.GetAudioClip();
                        pulse.Recalibrate();
                        source.time = 0;
                        source.Play();

                        while (!source.isPlaying) yield return 0;
                        CurrentSong = temp1;

                        timeRight = timer(source.clip.length);
                        while (source.isPlaying && skip != true && x == 0 && (PlayerPrefs.GetInt("MusicPlayer", 1) == 1))
                            yield return 0;
                    }
                    
                    skip = false;
                    skipSong = 0;
                    if (i == Paths.Count - 1)
                    {
                        i = -1;
                    }

                }
            }
        }
    }
    public void Mp3ToWav(string mp3File, string outputFile)
    {
        using (Mp3FileReader reader = new Mp3FileReader(mp3File))
        {
            WaveFileWriter.CreateWaveFile(outputFile, reader);
        }
    }
    public void Mp3ToWav(Mp3FileReader reader, string outputFile)
    {
        File.Delete(outputFile);
        WaveFileWriter.CreateWaveFile(outputFile, reader);
        run = true;
    }*/

    void DoMyWindow(int windowID)
    {
        GUILayout.Label("Current Song: " + CurrentSong);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Back"))
        {
            x = curr - 1;
            skip = true;
        }
        if (play == 1)
        {
            if (GUILayout.Button("Pause"))
                play = 0;
        }
        else
        {
            if (GUILayout.Button("Play"))
                play = 1;
        }
        if (GUILayout.Button("Skip"))
        {
            skip = true;
            x = curr + 1;
        }
        if (GUILayout.Button("PlayList")) showPL = !showPL;
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Volume (" + Mathf.RoundToInt(source.volume*100) + "%)", GUILayout.Width(windowRect.width/5));
        source.volume = GUILayout.HorizontalSlider(source.volume, 0f, 1f);
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Time (" + timeLeft + " of " + timeRight +")");
        /*source.time = GUILayout.HorizontalSlider(source.time, 0f, source.clip.length);*/

        GUI.DragWindow();
    }

    int view = 0;
    int viewChangeBy = 25;
    void ListWindow(int windowID)
    {
        GUILayout.Space(0);
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<"))
        {
            if (view > 0) view -= viewChangeBy;
            scrollPL = new Vector2();
        }
        if (GUILayout.Button(">"))
        {
            if (view + viewChangeBy < Paths.Count) view += viewChangeBy;
            scrollPL = new Vector2();
        }
        GUILayout.EndHorizontal();
        scrollPL = GUILayout.BeginScrollView(scrollPL, false, true, GUILayout.Width(windowRect.width-3), GUILayout.Height(windowRect.height - 30));
        
        for (int i = view; i < ((Paths.Count < view+viewChangeBy) ? Paths.Count : view + viewChangeBy); i++)
        {
            GUILayout.BeginHorizontal();
            string[] temp = Paths[i].Split('\\');
            GUILayout.Label(temp[temp.Length-1].Split(new string[] { ".mp3" }, StringSplitOptions.None)[0].Trim(), GUILayout.Width(windowRect.width*0.7f));
            if (GUILayout.Button("Play")) x = i;
            GUILayout.EndHorizontal();   
        }
        GUILayout.EndVertical();
        GUI.EndScrollView();
    }

    
    void OnGUI()
    {
        GUI.skin = skin;
        windowRect = GUI.Window(1, windowRect, DoMyWindow, "Music Player");
        if (showPL)
        {
            GUI.Window(2, new Rect(windowRect.x + windowRect.width, windowRect.y, windowRect.width, windowRect.height), ListWindow, "");
            if (windowRect.x < 0)
                windowRect.x = 0;
            if (windowRect.y < 0)
                windowRect.y = 0;
            if (windowRect.x > Screen.width - windowRect.width*2)
                windowRect.x = Screen.width - windowRect.width*2;
            if (windowRect.y > Screen.height - windowRect.height)
                windowRect.y = Screen.height - windowRect.height;
        }
        else {
            if (windowRect.x < 0)
                windowRect.x = 0;
            if (windowRect.y < 0)
                windowRect.y = 0;
            if (windowRect.x > Screen.width - windowRect.width)
                windowRect.x = Screen.width - windowRect.width;
            if (windowRect.y > Screen.height - windowRect.height)
                windowRect.y = Screen.height - windowRect.height;
        }
            
        
    }

    void LateUpdate()
    {
        this.GetComponent<AudioReverbFilter>().reverbPreset = settings.zone;
        this.GetComponent<AudioDistortionFilter>().distortionLevel = settings.Distortion;
        this.GetComponent<AudioHighPassFilter>().enabled = settings.HighOnly;
        this.GetComponent<AudioLowPassFilter>().enabled = settings.BassOnly;
        source.pitch = settings.PlaySpeed*play;

        timeLeft = timer(source.time);
    }

    void OnDisable()
    {
        PlayerPrefs.SetInt("MusicPosX", (int)windowRect.x);
        PlayerPrefs.SetInt("MusicPosY", (int)windowRect.y);
        //if (File.Exists(Application.persistentDataPath + "/temp.wav")) File.Delete(Application.persistentDataPath + "/temp.wav");
    }
    public string timer(float time)
    {
        string temp;
        if (time > 3600)
            temp = Mathf.FloorToInt(time / 3600) + "h " 
                + Mathf.FloorToInt((time - Mathf.FloorToInt(time / 3600) * 3600) / 60) + "m " 
                + Mathf.RoundToInt(time - Mathf.FloorToInt(time / 3600) * 3600) + "s";
        else if (time > 60)
            temp = Mathf.FloorToInt((time - Mathf.FloorToInt(time / 3600) * 3600) / 60) + "m " + Mathf.RoundToInt(time - Mathf.FloorToInt((time - Mathf.FloorToInt(time / 3600) * 3600) / 60) * 60) + "s";
        else
            temp = Mathf.RoundToInt(time) + "s";

        return temp;
    }

    Vector2 Fix(int x, int y, int FontSize = 0)
    {
        return new Vector2(x * (Screen.width) / 100, y * Screen.height / 100);
    }

    void Awake()
    {
        //Enable Callback on the main Thread
        UnityThread.initUnityThread();
    }

    string currentlyPlaying = "";
    string lastPlayedTitle = "";
    AudioClip lastPlayedAudioFile;
    float currentLength = 0;
    public void LoadSong(string musicPath)
    {
        Debug.Log(musicPath);
        string songTitle = Path.GetFileNameWithoutExtension(musicPath);
        ThreadPool.QueueUserWorkItem(delegate
        {
            //Set title of song
            if (songTitle != currentlyPlaying && songTitle != lastPlayedTitle)
            {
                //Parse the file with NAudio
                AudioFileReader aud = new AudioFileReader(musicPath);
                Debug.Log(aud.TotalTime);
                //Create an empty float to fill with song data
                float[] AudioData = new float[aud.Length];
                //Read the file and fill the float
                Debug.Log(musicPath);
                aud.Read(AudioData, 0, (int)aud.Length);

                //Now, create the clip on the main Thread and also play it
                UnityThread.executeInUpdate(() =>
                {
                    Debug.Log(musicPath);
                    //Create a clip file the size needed to collect the sound data
                    AudioClip craftClip = AudioClip.Create(songTitle, (int)aud.Length, aud.WaveFormat.Channels, aud.WaveFormat.SampleRate, false);
                    //Fill the file with the sound data
                    craftClip.SetData(AudioData, 0);

                    if (craftClip.loadState == AudioDataLoadState.Loaded && musicPath.Contains(songTitle))
                    {
                        playMusic(craftClip, songTitle);
                        lastPlayedTitle = currentlyPlaying;
                        lastPlayedAudioFile = craftClip;
                        currentlyPlaying = Path.GetFileNameWithoutExtension(musicPath);
                        timeRight = timer(currentLength = (float)aud.TotalTime.TotalSeconds);
                        /*Disposing on main thread may also introduce freezing so do that in a Thread too*/
                        ThreadPool.QueueUserWorkItem(delegate { aud.Dispose(); });
                    }
                });
            }
            else
            {
                UnityThread.executeInUpdate(() =>
                {
                    playMusic(lastPlayedAudioFile, lastPlayedTitle);
                });
            }
        });
    }

    void playMusic(AudioClip lpaf, string lpt)
    {
        source.time = 0;
        source.clip = lpaf;
        source.time = 0;
        pulse.Recalibrate();
        source.Play();
    }
}

public class MusicPlayerSettings
{
    public float PlaySpeed;
    public float Distortion;
    public bool BassOnly;
    public bool HighOnly;
    public AudioReverbPreset zone;

    public MusicPlayerSettings()
    {
        PlaySpeed = 1f;
        Distortion = 0;
        BassOnly = false;
        HighOnly = false;
        zone = AudioReverbPreset.Off;
    }

}
