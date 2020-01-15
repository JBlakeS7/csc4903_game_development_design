using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAB.Networking
{
    [RequireComponent(typeof(NetworkGameObject))]
    public abstract class NetworkComfortScript : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            if(GetComponent<NetworkGameObject>().isAuth == false)
            {
                Destroy(this);
            }
        }

    }
}