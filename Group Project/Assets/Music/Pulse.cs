using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour {
    public Material mat;
    public Light light;
    Color org = Color.white;
    int Modifier = 150;
    public static float m = 0.75f;
    public bool main = false;
    public AudioSource audio;
    static float[] bands = new float[8];

    [Range(0,7)]
    public int band = 0;

    private void Start()
    {
        org = Color.red;
        //org = mat.GetColor("_Color");
        //org = mat.GetColor("_EmissionColor");
        QualitySettings.vSyncCount = 0;
        StartCoroutine(run());
    }
    
    IEnumerator run()
    {
        
        float test = 0;
        float last = 0;
        float L = 0;
        Color lastColor = org;
        float smooth = 1f;
        mat.SetColor("_Color", Color.black);
        while (true)
        {
            if(main)
                GetData();
            test = bands[band];
            
            test *= 5f;
            if(test*m > 0.25f*m)
            L += test*m;
            //if (test > m)  L = 1;
            if (L > 1)  L = 1;

            if (L > 0)
            {
                Color finalColor = Color.Lerp(Color.black, org, L );
                mat.SetColor("_Color", finalColor);
                L -= Time.fixedDeltaTime*5f;
                if (L <= 0)
                {
                    mat.SetColor("_Color", Color.black);
                }
            }
            else
            {
                if (L < 0)
                {
                    mat.SetColor("_Color", Color.black);
                }
                L = 0;
            }

            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }
    }


    /*
    IEnumerator run()
    {
        
        float test = 0;
        float last = 0;
        float L = 0;
        Color lastColor = org;
        float smooth = 1f;
        while (true)
        {
            if(main)
                GetData();
            test = bands[band];
            
            test *= 10;
            if (test > m)
                L = 5;

            if (last != test)
            {
                if(light != null) light.intensity = Mathf.Lerp(light.intensity, 1 + test, Time.fixedDeltaTime * 5);
                smooth = Mathf.Lerp(smooth, 1+test, Mathf.Abs(test-smooth));
                Color finalColor = Color.Lerp(Color.black, org, smooth-m);
                mat.SetColor("_Color", finalColor);
            }
            last = test;

            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }
    }*/

    void GetData()
    {
        float[] spectrum = new float[512];
        audio.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            float average = 0;
            if(i == 7)
            {
                sampleCount += 2;
            }
            for(int j = 0; j < sampleCount; j++)
            {
                average += spectrum[count]*(count+1);
                count++;
            }
            average /= sampleCount;
            if(audio.volume > 0f)
            bands[i] = (average/5)/audio.volume;
        }


    }

    public void Recalibrate()
    {
        if (audio.clip != null)
        {
            m = 0;
            float[] samples = new float[audio.clip.samples * audio.clip.channels];
            audio.clip.GetData(samples, 0);
            for (int i = 0; i < samples.Length; i++)
            {
                float x = samples[i];
                if (x > m) m = x;
            }
        }
    }

    /*
    IEnumerator runOld()
    {
        float test = 0;
        float last = 0;
        while (true)
        {
            float[] spectrum = new float[256];

            AudioListener.GetSpectrumData(spectrum, channel, FFTWindow.Rectangular);

            for (int i = 1; i < 4; i++)
            {
                test += (spectrum[i - 1]);
            }
            test /= 3; //255
            if (last != test)
            {
                Color finalColor = org * ((test * Modifier));
                light.intensity = Mathf.Max(4 * ((test * Modifier)), 1);

                mat.SetColor("_Color", finalColor);
                //mat.SetColor("_EmissionColor", finalColor);
            }
            if (test > max)
            {
                max = test;
                Modifier = Mathf.FloorToInt(1 / max);
            }
            last = test;

            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
        }
    }

    */
    private void OnDisable()
    {

        mat.SetColor("_EmissionColor", org);
    }
}
