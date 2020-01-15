using UnityEngine;
using BIAB.Networking;

public class GameNetMsgHandler : MonoBehaviour 
{
    void Start()
    {
        Client c = NetworkManager.main.GetClient();
        if(c != null)
            Client.OnReceivedNetMsg += OnClientNetMsg;
        if(Server.main != null)
            Server.OnReceivedNetMsg += OnServerNetMsg;
    }

    void OnClientNetMsg(int cID, NetMsg msg) 
    {
        
    }

    void OnServerNetMsg(int cID, NetMsg msg)
    {

    }
}