using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BootCampMode : MonoBehaviour
{
    public Vector3 StartPosition;
    public Vector3 StartRotation;
    public GameObject PlayerPrefab;

    
    public TextMeshPro GrenadeGameScore;

    public GameObject loadedPlayer;


    void Start()
    {
        loadedPlayer = Instantiate(PlayerPrefab);
        loadedPlayer.transform.position = StartPosition;
        loadedPlayer.transform.eulerAngles = StartRotation;
    }

    void Update()
    {
        if (GrenadeGameScore != null)
            GrenadeGameScore.text = "Grenade Score: " + GrenadeScore.score;
        
    }
}
