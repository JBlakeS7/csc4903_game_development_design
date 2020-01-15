using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAB.Networking
{
    [RequireComponent(typeof(NetworkManager))]
    public class NetworkManagerHUD : MonoBehaviour
    {
        private NetworkManager manager;
        private void Start()
        {
            manager = GetComponent<NetworkManager>();
        }
        private void OnGUI()
        {
            if (GUI.Button(new Rect(50, 0, 150, 50), "Start Local Client"))
                //manager.StartLocalClient(false);
                manager.StartClient("10.129.193.251", 8080, false);

            //if (GUI.Button(new Rect(50, 150, 150, 50), "Start Host")) manager.StartHost();

            if (GUI.Button(new Rect(50, 75, 150, 50), "Start Server"))
                manager.StartServer(25566);

            if (GUI.Button(new Rect(200, 0, 150, 50), "Stop Local Client"))
                manager.StopClient();

            //if (GUI.Button(new Rect(200, 150, 150, 50), "Stop Host")) manager.StopHost();

            if (GUI.Button(new Rect(200, 75, 150, 50), "Stop Server"))
                manager.StopServer();
        }
    }
}