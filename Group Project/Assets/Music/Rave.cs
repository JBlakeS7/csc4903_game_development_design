using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rave : MonoBehaviour
{
    public Color org = Color.white;
    public int Modifier = 150;
    public AudioSource audio;
    static float[] bands = new float[8];

    public Light[] lights;


    [Range(0, 7)]
    public int band = 1;

    bool running = false;
    private void Update()
    {
        if (ShootToRave.Raving && running == false)
        {
            running = true;
            Run();
        }
    }

    void Run()
    {
        org = Color.red;
        audio = ShootToRave.audio;
        StartCoroutine(run());
    }


    private float colorChange = 0;
    IEnumerator run()
    {
        colorChange = Time.time;
        float test = 0;
        float L = 0;
        Color lastColor = org;
        SetLights(Color.blue);
        SetLights(1.5f);
        while (true)
        {
            GetData();
            test = bands[band];

            if (colorChange + 1 < Time.time)
            {
                org = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
                colorChange = Time.time;
                SetLights(org);
            }

            test *= Modifier;
            L += test;
            //if (test > m)  L = 1;
            if (L > 2) L = 2;

            if (L > 0)
            {
                //SetLights(L);
                L -= Time.fixedDeltaTime * 5f;
                if (L <= 0)
                {
                    //SetLights(0);
                }
            }
            else
            {
                if (L < 0)
                {
                    //SetLights(0);
                }
                L = 0;
            }

            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }
    }

    void SetLights(float inten)
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].intensity = inten;
        }

    }

    void SetLights (Color color)
    {
        for (int i = 0; i < lights.Length; i++) { 
            lights[i].color = color;
            }

    }

    void GetData()
    {
        float[] spectrum = new float[512];
        audio.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            float average = 0;
            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                average += spectrum[count] * (count + 1);
                count++;
            }
            average /= sampleCount;
            if (audio.volume > 0f)
                bands[i] = (average / 5) / audio.volume;
        }


    }
    
    private void OnDisable()
    {

        SetLights(Color.white);
    }
}
