using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScore : MonoBehaviour
{
    public static int score = 0;
    private void Start()
    {
        score = 0;
    }

    public int pointsForThis = 1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("ade"))
        {
            score += pointsForThis;
        }
    }
}
