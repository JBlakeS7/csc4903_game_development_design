using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitMarker : MonoBehaviour
{
    private static float time = 0;
    public static void Hit()
    {
        time =  1;
    }

    private Image image;
    private void Start()
    {
        image = this.GetComponent<Image>();
    }
    void Update()
    {
        if(time > 0f)
        {
            image.color = new Color(1, 1, 1, time -= Time.deltaTime);
        }
        else
        {
            image.color = Color.clear;
        }
    }
}
