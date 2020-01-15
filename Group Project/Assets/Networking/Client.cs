using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BIAB.Networking
{
    public abstract class Client
    {
        public string serverIP;
        public int maxUser;
        public int port;
        public int connectionID
        {
            get;
            protected set;
        }
        protected bool _isConnected = false;
        public bool isHost = false;
        protected Dictionary<int, GameObject> networkedObjects;

        
        public delegate void ReceivedNetMsg(int cID, NetMsg msg);
        public static event ReceivedNetMsg OnReceivedNetMsg;
        public void OnReceivedNetMsgCall(int cID, NetMsg msg)
        {
            OnReceivedNetMsg(cID, msg);
        }

        public bool isConnected
        {
            get
            {
                return _isConnected;
            }
        }
        public abstract void Init(int maxUser, int port, string serverIP, bool isHost);


        public abstract void Close();

        public abstract void NetUpdate();

        public abstract void OnConnected();
        public abstract void OnDisconnected();
        public abstract void OnReceiveMessage(NetMsg msg);
        public abstract void SendMessage(NetMsg msg);
    }
}