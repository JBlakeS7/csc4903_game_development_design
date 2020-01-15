using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    public static MapData main;
    public void Awake()
    {
        main = this;
    }
    public GameObject[] FFASpawns;
    public GameObject[] TDMRedSpawns;
    public GameObject[] TDMBlueSpawns;
    public GameObject[] CTFFlagSpawns;
    public GameObject[] HordeSpawn;
    public GameObject PlayerHordeSpawn;
}
