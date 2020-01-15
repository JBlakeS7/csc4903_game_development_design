using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAB.Networking
{
    public abstract class Server
    {
        public static Server main;

        public int maxUser;
        public int port;
        public int UserCount
        {
            get
            {
                return clientIDs.Count;
            }
        }
        public List<int> clientIDs
        {
            get;
            protected set;
        }
        public Dictionary<int, ClientInfo> clientInfo
        {
            get;
            protected set;
        }
        protected Dictionary<int, GameObject> networkedObjects;
        protected Dictionary<int, NetworkTransform> networkedTransformObjects;
        
        public delegate void ReceivedNetMsg(int cID, NetMsg msg);
        public static event ReceivedNetMsg OnReceivedNetMsg;

        public void OnReceivedNetMsgCall(int cID, NetMsg msg)
        {
            OnReceivedNetMsg(cID, msg);
        }

        protected bool _isStarted = false;
        public bool isStarted
        {
            get
            {
                return _isStarted;
            }
        }


        public abstract void Init(int maxUser, int port);


        public abstract void Shutdown();
        public abstract void NetUpdate();
        public abstract void GameComplete();
        public abstract void OnClientConnected(int id);
        public abstract void OnClientDisconnected(int id);
        public abstract void OnClientMessage(int id, NetMsg msg);
        public abstract void SendMessage(NetMsg msg, int connectionID);
        public abstract void BroadcastMessage(NetMsg msg);

        public struct ClientInfo
        {
            public ClientState state;

            public ClientInfo(ClientState state = ClientState.Joined)
            {
                this.state = state;
            }

        }
        public enum ClientState
        {
            Joined,
            Voting,
            Voted,
            Loading,
            Loaded,
            Playing
        }
    }

    [Serializable]
    public class NetMsg
    {
        public int type;
        public NetMsg()
        {
            type = NetOP.NONE;
        }
    }
    [Serializable]
    public class TextNetMsg : NetMsg
    {
        public string text;

        public TextNetMsg(string t)
        {
            type = NetOP.TEXT;
            text = t;
        }
    }


    public static class NetStat
    {
        public const int BUFFER_SIZE = 1024;
    }
}

