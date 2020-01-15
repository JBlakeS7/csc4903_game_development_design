using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BIAB.Networking;
using Unity.Mathematics;
using System;

public static class NetOP
{
    public const int NONE = 0;
    public const int TEXT = 1;
    public const int TRANSFORM = 2;
    public const int DEATH = 3;
    public const int SHOOT = 4;
    public const int INPUT = 5;
    public const int VOTE = 6;
    public const int GAMEMANAGER = 7; // What map and gamemode to load to
    public const int JUMP = 8;
    public const int LOADED = 9; // Client is ready for game to start
    public const int STATE = 10; // Server State of Playing, Waiting for Clients To Load, Not Enough Players, Loading
    public const int SPAWN = 11; // Spawn object with id
    public const int SWAPWEAPON = 12; // Swap Weapon
    public const int GAMESTATE = 13; // Start, EndGame, Middle/Text
    public const int GAMEMODEDATA = 14; // Data The Gamemode NetMsg The Gamemode itself handles like health changes
}

[Serializable]
public class TransformNetMsg : NetMsg
{
    public float3 position;
    public float3 rotation;
    public int connectionID;

    public TransformNetMsg(float3 position, float3 rotation, int connectionID)
    {
        this.position = position;
        this.rotation = rotation;
        this.connectionID = connectionID;
        type = NetOP.TRANSFORM;
    }
}

[Serializable]
public class DeathNetMsg : NetMsg
{
    public int killed;
    public int killer;

    public DeathNetMsg(int killed, int killer)
    {
        this.killed = killed;
        this.killer = killer;
        type = NetOP.DEATH;
    }
}

[Serializable]
public class VoteNetMsg : NetMsg
{
    public int gameMode;
    public int map;

    public VoteNetMsg(int gameMode, int map)
    {
        this.map = map;
        this.gameMode = gameMode;
        type = NetOP.VOTE;
    }
}

[Serializable]
public class GameManagerNetMsg : NetMsg
{
    public int gameMode;
    public int map;

    public GameManagerNetMsg(int gameMode, int map)
    {
        this.map = map;
        this.gameMode = gameMode;
        type = NetOP.GAMEMANAGER;
    }
}

[Serializable]
public class PlayerInputNetMsg : NetMsg
{
    public PlayerInputNetMsg(float x, float y)
    {
        type = NetOP.INPUT;
        direction = new float2(x, y);
    }
    public PlayerInputNetMsg(float2 input)
    {
        type = NetOP.INPUT;
        direction = input;
    }
    public float2 direction;
}

[Serializable]
public class PlayerJumpInputNetMsg : NetMsg
{
    public PlayerJumpInputNetMsg()
    {
        type = NetOP.JUMP;
    }
}

[Serializable]
public class PlayerLoadedNetMsg : NetMsg
{
    public int id;
    public PlayerLoadedNetMsg(int connectionID)
    {
        id = connectionID;
        type = NetOP.LOADED;
    }
}

[Serializable]
public class ServerStateNetMsg : NetMsg
{
    public int state;
    public ServerStateNetMsg(int state)
    {
        type = NetOP.STATE;
        this.state = state;
    }
}

[Serializable]
public class PlayerShootInputNetMsg : NetMsg
{
    public PlayerShootInputNetMsg(float x, float y, float z)
    {
        type = NetOP.SHOOT;
        direction = new float3(x, y, z);
    }
    public PlayerShootInputNetMsg(float3 input)
    {
        type = NetOP.SHOOT;
        direction = input;
    }
    float3 direction;
}