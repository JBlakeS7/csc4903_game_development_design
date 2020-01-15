using System.Collections;
using System.Collections.Concurrent;
using Unity.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BIAB.Networking
{
    public class NetworkManager : MonoBehaviour
    {
        public static NetworkManager main;

        public int port = 25566;

        private void Awake()
        {
            if (main == null && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Networking") == false)
            {
                main = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this);
            }
        }

        private Server server;
        private Client client;

        public Client GetClient()
        {
            return client;
        }
        public bool IsServer()
        {
            return (server != null);
        }

        private void Update()
        {
            
            if (server != null)
                server.NetUpdate();
            if(client != null)
                client.NetUpdate();
            
        }


        public void StartServer(int port)
        {
            
            if (server == null)
            {
                server = new Server_LLAPI();
                server.Init(100, port);
            }else if(server.isStarted == false)
            {
                server.Init(100, port);
            }

            if(client != null)
            {
                client.isHost = true;
            }
        }

        public void StopServer()
        {
            if(server != null)
            {
                if (server.isStarted)
                {
                    StopClient();
                    server.Shutdown();
                    Debug.Log("Server Shutdown");
                }
                else
                    Debug.Log("Server Already Offline");
            }
            else
                Debug.Log("Server Never Allocated");
        }
        public void StartClient(string serverIP, int port, bool isHost)
        {
            client = new Client_LLAPI();
            if (server != null)
                client.Init(100, port, serverIP, isHost || server.isStarted);
            else
                client.Init(100, port, serverIP, isHost);
        }
        public void StartLocalClient(bool isHost)
        {
            StartClient("127.0.0.1", port, isHost);
        }
        public void StopClient()
        {
            if (client != null)
            {
                if (client.isConnected) {
                    client.Close();
                    Debug.Log("Client Closed");


                }
            }
            else
                Debug.Log("Client Never Allocated");
        }
        public void StartHost()
        {
            StartServer(port);
            StartLocalClient(true);
        }
        public void StopHost()
        {
            StopClient();
            StopServer();
        }

    }
}
