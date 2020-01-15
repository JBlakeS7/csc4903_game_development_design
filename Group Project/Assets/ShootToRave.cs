using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootToRave : MonoBehaviour
{
    public static bool Raving = false;

    public static int total = 0;
    public static AudioSource audio;

    public AudioClip clip;

    private Light light;
    private bool lightOn = true;
    private void Awake()
    {
        total += 1;
    }

    private void Start()
    {
        light = GetComponent<Light>();
    }

    private void Update()
    {
        if (Raving && lightOn == false)
            lightOn = light.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name.Contains("ullet") && lightOn == true && Raving == false)
        {
            total -= 1;
            lightOn = light.enabled = false;
            if (total == 0)
            {
                if(audio == null)
                    audio = HordeMode.main.loadedPlayer.AddComponent<AudioSource>();
                Raving = true;
                audio.clip = clip;
                audio.Play();
            }
        }
    }
}
