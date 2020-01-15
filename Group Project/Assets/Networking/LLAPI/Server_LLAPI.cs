using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

namespace BIAB.Networking
{
    public class Server_LLAPI : Server
    {
        private byte reliableChannel;
        private int hostID;
        private int webHostID;

        public override void BroadcastMessage(NetMsg msg)
        {
            foreach(int id in clientIDs)
            {
                SendMessage(msg, id);
            }
        }

        public override void GameComplete()
        {
            foreach (KeyValuePair<int, ClientInfo> c in clientInfo)
            {
                ClientInfo i = c.Value;
                i.state = ClientState.Voting;
                clientInfo[c.Key] = i;
            }
        }

        public override void Init(int maxUser, int port)
        {
            main = this;
            NetworkTransport.Init();
            clientInfo = new Dictionary<int, ClientInfo>();
            clientIDs = new List<int>();
            ConnectionConfig cc = new ConnectionConfig();
            reliableChannel = cc.AddChannel(QosType.Reliable);
            this.maxUser = maxUser;
            this.port = port;
            HostTopology htp = new HostTopology(cc, maxUser);

            hostID = NetworkTransport.AddHost(htp, port, null);
            webHostID = NetworkTransport.AddWebsocketHost(htp, port + 1, null);

            Debug.Log(string.Format("Opening Server on Ports {0} and {1}", port, port + 1));
            
            _isStarted = true;
        }

        public override void NetUpdate()
        {
            if (!_isStarted)
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
                    OnClientConnected(connectionId);
                    break;
                case NetworkEventType.DisconnectEvent:
                    OnClientDisconnected(connectionId);
                    break;
                case NetworkEventType.DataEvent:
                    
                        BinaryFormatter binaryFormatter = new BinaryFormatter();
                        MemoryStream ms = new MemoryStream(refBuffer);
                        NetMsg msg = (NetMsg)binaryFormatter.Deserialize(ms);
                        OnClientMessage(connectionId, msg);
                        OnReceivedNetMsgCall(connectionId, msg);
                    break;
                case NetworkEventType.BroadcastEvent:
                default:
                    Debug.Log("Unknown Network Event");
                    break;
            }
        }

        public override void OnClientConnected(int id)
        {
            Debug.Log("Client Connected with ID:" + id);
            clientIDs.Add(id);
            clientInfo.Add(id, new ClientInfo());
        }

        public override void OnClientDisconnected(int id)
        {
            Debug.Log("Client Disconnected with ID:" + id);
            clientIDs.Remove(id);
            clientInfo.Remove(id);
        }

        public override void OnClientMessage(int id, NetMsg msg)
        {
            switch (msg.type)
            {
                case NetOP.NONE:
                    break;
                case NetOP.TEXT:
                    string text = ((TextNetMsg)msg).text;
                    Debug.Log("Received Text On Server From ID:"+id);
                    Debug.Log(text);
                    BroadcastMessage((TextNetMsg)msg);
                    SendMessage(msg, id);
                    break;
                case NetOP.INPUT:
                    Debug.Log("Player Moved");

                    break;
                case NetOP.JUMP:
                    Debug.Log("Player Jumped");

                    break;
                case NetOP.SHOOT:
                    Debug.Log("Player Shot");

                    BroadcastMessage((PlayerShootInputNetMsg)msg);
                    break;
                case NetOP.VOTE:
                    Debug.Log("Player Voted");
                    if(clientInfo[id].state == ClientState.Voting)
                    {
                        ClientInfo i = clientInfo[id];
                        i.state = ClientState.Voted;
                        clientInfo[id] = i;
                    }

                    break;
                case NetOP.LOADED:
                    Debug.Log("Player is Loaded");
                    if (clientInfo[id].state == ClientState.Loading)
                    {
                        ClientInfo i = clientInfo[id];
                        i.state = ClientState.Loaded;
                        clientInfo[id] = i;
                    }
                    BroadcastMessage((PlayerLoadedNetMsg)msg);
                    break;
                default:
                    Debug.Log("Undefined Behaviour with type:" + msg.type);
                    break;
            }
            
        }

        public override void SendMessage(NetMsg msg, int connectionID)
        {
            if (clientInfo[connectionID].state == ClientState.Joined && msg.type != 6) return;
            byte[] buffer = new byte[NetStat.BUFFER_SIZE];
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(buffer);
            binaryFormatter.Serialize(ms, msg);
            byte error;
            NetworkTransport.Send(hostID, connectionID, reliableChannel, buffer, buffer.Length, out error);
        }

        public override void Shutdown()
        {
            _isStarted = false;
            NetworkTransport.Shutdown();
        }
    }
}