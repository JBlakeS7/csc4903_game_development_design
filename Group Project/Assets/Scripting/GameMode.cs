using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BIAB.Networking;

/// <summary>
/// Handles Game Behavior
/// Receives commands from NetworkManager and GameManager
/// </summary>
public abstract class GameMode : MonoBehaviour
{
    public string Name
    {
        get;
        protected set;
    }
    public List<int> connectionIDs;
    protected float startTime = 0;
    public int ElapsedTime
    {
        get
        {
            if (startTime == 0)
                return 0;
            return (int)(Time.fixedTime - startTime);
        }
    }

    public abstract void SpawnPlayer(int connectionID);

    public abstract void StartMatch(); // Start Match
    public abstract void StopMatch(); // Stop Match

    public abstract bool isFinished(); // Is the match over
}


