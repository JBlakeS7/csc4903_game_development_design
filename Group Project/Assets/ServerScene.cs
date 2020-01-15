using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using BIAB.Networking;
using TMPro;

[RequireComponent(typeof(NetworkManager))]
public class ServerScene : MonoBehaviour
{
    private NetworkManager nm;

    public TextMeshProUGUI PlayerTab;
    public TextMeshProUGUI InfoTab;
    public TextMeshProUGUI AdminTab;

    void Start()
    {
        nm = GetComponent<NetworkManager>();
        nm.StartServer(25566);
        StartCoroutine(UpdateGUI());
        AdminTab.text = "Stop the server by clicking \"Close Server\"";
    }

    /// <summary>
    /// Hello
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateGUI()
    {
        while (true)
        {
            string p = "";
            int c = Server.main.UserCount;
            for (int i = 0; i < c; i++)
                p += "Client: " + i + "\n";
            PlayerTab.text = p;
            InfoTab.text = "Local IP: " + LocalIPAddress() + "\nGame Mode: " + GameManager.main.loadedGameMode.Name + "\nRound Elapsed Time: " + timer(GameManager.main.loadedGameMode.ElapsedTime) +"\nPlayer Count: " + c;

            yield return new WaitForSecondsRealtime(0.25f);
        }
    }

    private string timer(float time)
    {
        string temp;
        if (time > 3600)
            temp = Mathf.FloorToInt(time / 3600) + "h "
                + Mathf.FloorToInt((time - Mathf.FloorToInt(time / 3600) * 3600) / 60) + "m "
                + Mathf.RoundToInt(time - Mathf.FloorToInt(time / 3600) * 3600) + "s";
        else if (time > 60)
            temp = Mathf.FloorToInt((time - Mathf.FloorToInt(time / 3600) * 3600) / 60) + "m " + Mathf.RoundToInt(time - Mathf.FloorToInt((time - Mathf.FloorToInt(time / 3600) * 3600) / 60) * 60) + "s";
        else
            temp = Mathf.RoundToInt(time) + "s";

        return temp;
    }
    public void Stop()
    {
        nm.StopServer();
        Application.Quit();
        Debug.Break();
    }


    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }

}