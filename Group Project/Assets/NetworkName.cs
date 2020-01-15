using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class NetworkName : MonoBehaviour
{
    public GameObject[] Enables;
    public GameObject[] Disables;

    public SearchMatchmaking sm;
    public static bool EagleNet = false;

    // Start is called before the first frame update
    void Start()
    {
        NetworkInterface[] g = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface x in g)
        {
            if (x.Name.Contains("Eagle Net"))
            {
                EagleNet = true;
                foreach(GameObject a in Enables)
                {
                    a.SetActive(true);
                }
            }
        }

        if(EagleNet == false)
        {
            foreach (GameObject a in Disables)
            {
                a.SetActive(false);
            }

            if(sm != null)
            sm.enabled = true;
        }
    }
}
