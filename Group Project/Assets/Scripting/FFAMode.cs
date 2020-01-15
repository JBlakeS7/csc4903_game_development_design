using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BIAB.Networking;

public class FFAMode : GameMode
{
    Dictionary<int, PlayerInfo> players = new Dictionary<int, PlayerInfo>();

    private void Awake()
    {
        Name = "Free For All - DLC";
    }
    public override bool isFinished()
    {
        throw new System.NotImplementedException();
    }

    public override void SpawnPlayer(int connectionID)
    {
        players.Add(connectionID, new PlayerInfo(30f, 0, 0));
    }

    public override void StartMatch()
    {
        startTime = Time.fixedTime;
        foreach(int id in connectionIDs)
        {
            SpawnPlayer(id);
        }
    }

    public override void StopMatch()
    {
        throw new System.NotImplementedException();
    }

    bool isServer = false;
    // Start is called before the first frame update
    void Start()
    {
        isServer = NetworkManager.main.IsServer();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(KeyValuePair<int, PlayerInfo> p in players)
        {
            PlayerInfo x = p.Value;
            x.time -= Time.deltaTime;
            players[p.Key] = x;
            if (x.time < 0)
            {
                if (isServer) {
                    Server.main.BroadcastMessage(new DeathNetMsg(p.Key, -5));
                        }
            }
        }
    }

    struct PlayerInfo
    {
        public float time; // Health
        public int deaths;
        public int kills;

        public PlayerInfo(float time, int kills, int deaths)
        {
            this.time = time;
            this.kills = kills;
            this.deaths = deaths;
        }
    }
}

