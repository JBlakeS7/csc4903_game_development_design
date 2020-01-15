using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PracticeMode : GameMode
{
    public Vector3 StartPosition = new Vector3(5,1.5f,-10);
    public Vector3 StartRotation;
    private GameObject localPlayerObject;

    public override bool isFinished()
    {
        return false;
    }

    private void OnEnable()
    {
        StartMatch();
    }

    public override void SpawnPlayer(int connectionID)
    {
        localPlayerObject = GameObject.Instantiate(GameManager.main.PlayerPrefab);
        localPlayerObject.transform.position = StartPosition;
        localPlayerObject.transform.eulerAngles = StartRotation;
    }

    public override void StartMatch()
    {
        SpawnPlayer(0);
    }
    

    public override void StopMatch()
    {
        Destroy(localPlayerObject);
    }


    
}
