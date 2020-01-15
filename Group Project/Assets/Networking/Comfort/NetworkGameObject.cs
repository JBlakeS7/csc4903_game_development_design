using UnityEngine;

namespace BIAB.Networking
{
    public class NetworkGameObject : MonoBehaviour
    {
        private int _ID = -1;
        public bool local = false;
        public bool isAuth = false;
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID == -1)
                    _ID = value;
                else
                    Debug.LogError("Networked Object ID:" + _ID + " is Already Tracked!");
            }
        }
    }
}