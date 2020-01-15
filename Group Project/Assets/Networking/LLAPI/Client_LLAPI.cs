using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

namespace BIAB.Networking
{
    public class Client_LLAPI : Client
    {
        private byte reliableChannel;
        private int hostID;
        private byte error;

        public override void Init(int maxUser, int port, string serverIP, bool isHost)
        {
            if (_isConnected)
            {
                Debug.Log("Connection Already Established!");
                return;
            }
            this.isHost = isHost;
            NetworkTransport.Init();

            ConnectionConfig cc = new ConnectionConfig();
            reliableChannel = cc.AddChannel(QosType.Reliable);
            this.maxUser = maxUser;
            this.port = port;
            this.serverIP = serverIP;
            HostTopology htp = new HostTopology(cc, maxUser);

            hostID = NetworkTransport.AddHost(htp, 0);

#if UNITY_WEBGL && !UNITY_EDITOR
            connectionID = NetworkTransport.Connect(hostID, serverIP, port + 1, 0, out error);
#else
            connectionID = NetworkTransport.Connect(hostID, serverIP, port, 0, out error);
#endif
            Debug.Log(string.Format("Opening Client to {0}:{1}", serverIP, port));
            if(error == 0)
            {
                _isConnected = true;
            }
        }


        public override void Close()
        {
            _isConnected = false;
            if(!isHost) NetworkTransport.Shutdown();
        }

        public override void NetUpdate()
        {
            if (!_isConnected)
                return;

            int refHostId;
            int connectionId;
            int channelID;
            byte[] refBuffer = new byte[NetStat.BUFFER_SIZE];
            int dataSize;
            byte error;

            NetworkEventType type = NetworkTransport.Receive(out refHostId, out connectionId, out channelID, refBuffer, NetStat.BUFFER_SIZE, out dataSize, out error);
            switch (type)
            {
                case NetworkEventType.Nothing:
                    return;
                case NetworkEventType.ConnectEvent:
                    OnConnected();
                    break;
                case NetworkEventType.DisconnectEvent:
                    OnDisconnected();
                    Close();
                    break;
                case NetworkEventType.DataEvent:
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    MemoryStream ms = new MemoryStream(refBuffer);
                    NetMsg msg = (NetMsg)binaryFormatter.Deserialize(ms);
                    OnReceiveMessage(msg);
                    OnReceivedNetMsgCall(connectionID, msg);
                    break;
                case NetworkEventType.BroadcastEvent:
                default:
                    Debug.Log("Unknown Network Event");
                    break;
            }
        }

        public override void OnConnected()
        {
            Debug.Log("Local Client Connected");
            SendMessage(new TextNetMsg("HEWWO?!?!"));
        }

        public override void OnDisconnected()
        {
            Debug.Log("Local Client Disconnected");
        }

        public override void OnReceiveMessage(NetMsg msg)
        {
            switch (msg.type)
            {
                case NetOP.NONE:
                    break;
                case NetOP.TEXT:
                    string text = ((TextNetMsg)msg).text;
                    Debug.Log("Received Text On Client");
                    Debug.Log(text);
                    break;
                case NetOP.TRANSFORM:
                    Debug.Log("Player Moved");
                    break;
                case NetOP.DEATH:
                    Debug.Log("Player Died");
                    break;
                case NetOP.GAMEMANAGER:
                    Debug.Log("GameMode Changed");
                    break;
                case NetOP.LOADED:
                    Debug.Log("Player Loaded");
                    break;
                case NetOP.STATE:
                    Debug.Log("Server State Change");
                    break;
                case NetOP.SHOOT:
                    Debug.Log("Player Shot");
                    break;

                default:
                    Debug.Log("Undefined Behaviour with type:" + msg.type);
                    break;
            }

        }
    

        public override void SendMessage(NetMsg msg)
        {
            byte[] buffer = new byte[NetStat.BUFFER_SIZE];
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(buffer);
            binaryFormatter.Serialize(ms, msg);
            byte error;
            NetworkTransport.Send(hostID, connectionID, reliableChannel, buffer, buffer.Length, out error);
        }
    }
}