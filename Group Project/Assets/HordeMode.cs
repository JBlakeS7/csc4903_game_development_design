using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HordeMode : MonoBehaviour
{
    public static HordeMode main;

    public int Grenade = 0;

    // Main Setup
    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;
    public CanvasGroup Blackout;
    public CanvasGroup Loading;
    public ProgressBar LoadingBar;
    public CanvasGroup DeathScreen;

    // HUD
    public CanvasGroup PlayerHUD;
    public UnityEngine.UI.Text roundText;
    public UnityEngine.UI.Text healthText;
    public UnityEngine.UI.Slider healthBar;

    // HUD Wave
    public CanvasGroup Wave;
    public UnityEngine.UI.Text roundTimeText;

    public GameObject loadedPlayer;
    public Health playerHealth;
    public int round = 0;

    private List<GameObject> zombies;


    private void Awake()
    {
        main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        zombies = new List<GameObject>();
        LoadingBar.currentPercent = 0;
        FadeEffect.FadeCanvasIn(Blackout);
    }

    public void Colosseum()
    {
        FadeEffect.FadeCanvasIn(Loading);
        StartCoroutine(_startC());
    }

    public void Forest()
    {
        FadeEffect.FadeCanvasIn(Loading);
        StartCoroutine(_startF());
    }

    IEnumerator _startC()
    {
        yield return 0;
        yield return 0;
        yield return 0;
        AsyncOperation k = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Colosseum");
        while (k.isDone == false)
        {
            LoadingBar.currentPercent = k.progress * 100;
            yield return 0;
        }
        yield return 0;
        SpawnPlayer();
        LoadingBar.currentPercent = 100;
        FadeEffect.FadeCanvasOut(Blackout);
        FadeEffect.FadeCanvasOut(Loading);
        FadeEffect.FadeCanvasIn(PlayerHUD);
        SpawnWave(6);
    }

    IEnumerator _startF()
    {
        yield return 0;
        yield return 0;
        yield return 0;
        AsyncOperation k = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("The Forest");
        while (k.isDone == false)
        {
            LoadingBar.currentPercent = k.progress * 100;
            yield return 0;
        }
        yield return 0;
        SpawnPlayer();
        LoadingBar.currentPercent = 100;
        FadeEffect.FadeCanvasOut(Blackout);
        FadeEffect.FadeCanvasOut(Loading);
        FadeEffect.FadeCanvasIn(PlayerHUD);
        SpawnWave(6);
    }

    void SpawnPlayer()
    {
        loadedPlayer = Instantiate(PlayerPrefab);
        playerHealth = loadedPlayer.AddComponent<Health>();
        playerHealth.healthText = healthText;
        playerHealth.healthBar = healthBar;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Forest"))
            playerHealth.health = 60;
        loadedPlayer.transform.position = MapData.main.PlayerHordeSpawn.transform.position;
        loadedPlayer.transform.rotation = MapData.main.PlayerHordeSpawn.transform.rotation;
    }

    private bool dead = false;
    public void PlayerDied()
    {
            dead = true;
            Debug.Log("PlayerDied");
            StopAllCoroutines();
            for (int i = 0; i < zombies.Count;)
            {
                Destroy(zombies[0]);
                zombies.RemoveAt(0);
            }
            StartCoroutine(AfterDeath());
        
    }
    IEnumerator AfterDeath()
    {
        FadeEffect.FadeCanvasIn(DeathScreen);

        yield return new WaitForSecondsRealtime(5f);
        FadeEffect.FadeCanvasOut(DeathScreen, 1f);
        loadedPlayer.transform.position = MapData.main.PlayerHordeSpawn.transform.position;
        loadedPlayer.transform.rotation = MapData.main.PlayerHordeSpawn.transform.rotation;
        Grenade = 1;
        round = 0;
        playerHealth.health = 30;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Forest"))
            playerHealth.health = 60;
        SpawnWave(6);
    }


    void SpawnWave(int x)
    {
        dead = false;
        round += 1;
        if(roundText != null)
            roundText.text = "Round: " + round;
        int c = MapData.main.HordeSpawn.Length;
        Debug.Log(c);
        for (int i = 0; i< x; i++)
        {
            int a = -1;
            if (c > 1) a = Random.Range(0, c - 1);

            if (c == 1)
                a = 0;
            GameObject temp = Instantiate(EnemyPrefab);
            temp.GetComponent<AI1>().health += Mathf.CeilToInt(Mathf.Log(round) * Mathf.Log(round));
            if (a == -1)
            {
                temp.transform.position = new Vector3(0, 1, 0);
            }
            else
            {
                while((loadedPlayer.transform.position- MapData.main.HordeSpawn[a].transform.position).magnitude < 5f)
                    a = Random.Range(0, c - 1);
                temp.transform.position = MapData.main.HordeSpawn[a].transform.position;
                temp.transform.rotation = MapData.main.HordeSpawn[a].transform.rotation;
            }
            zombies.Add(temp);
            temp.GetComponent<NavMeshAgent>().enabled = true;
        }
        StartCoroutine(checkForNext());
    }
    
    public void RemoveZombie(GameObject zom)
    {
        playerHealth.health += 3;
        zombies.Remove(zom);
    }

    IEnumerator checkForNext()
    {
        while (zombies.Count > 0) yield return 0;
        if (!dead)
        StartCoroutine(timeForNext());
    }
    IEnumerator timeForNext()
    {
        playerHealth.timer = false;
        FadeEffect.FadeCanvasIn(Wave);
        for(int i = 10; i >= 0; i--)
        {
            roundTimeText.text = "Time Till Wave: " + i+"s";
            while (PauseMenu.paused) yield return 0;
            if (dead) break;
            yield return new WaitForSecondsRealtime(1);
        }
        FadeEffect.FadeCanvasOut(Wave);
        if (!dead)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Forest"))
                SpawnWave(round * 12);
            else
                SpawnWave(round * 6);
            playerHealth.timer = true;
        }
    }
}
