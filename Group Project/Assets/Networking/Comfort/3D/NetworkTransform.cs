using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAB.Networking
{
    [RequireComponent(typeof(NetworkGameObject))]
    public class NetworkTransform : MonoBehaviour
    {
        public int UpdatesPerSecond = 10;
        public bool interpolate = true;
        Client client;
        void Start()
        {
            NetworkGameObject ngo = GetComponent<NetworkGameObject>();
            if (ngo.isAuth && ngo.local) // Local means controlled by local
            {
                client = NetworkManager.main.GetClient();
                StartCoroutine(NetUpdate());
            }
        }

        IEnumerator NetUpdate()
        {
            if (client != null)
            {
                while (true)
                {
                    client.SendMessage(new TransformNetMsg((Unity.Mathematics.float3)this.transform.position, (Unity.Mathematics.float3)this.transform.eulerAngles, client.connectionID));
                    yield return new WaitForSecondsRealtime(1f / UpdatesPerSecond);
                }
            }
        }


        private Vector3 targetP; // Position Received
        private Vector3 targetR; // Rotation Received
        private float targetT; // Time Received
        private Vector3 targetV; // Calculated Velocity;
        public void SetTarget(Vector3 Position, Vector3 Rotation)
        {
            if(!interpolate){
                 this.transform.position = Position;
                 this.transform.eulerAngles = Rotation;
            }else{
                targetV = (this.transform.position-Position)*(Time.time - targetT);
                targetT = Time.time;
                targetP = Position;
                targetR = Rotation;
            }
        }

        private void Update() 
        {
            if(interpolate)
            {
                if(targetT+ 1/UpdatesPerSecond > Time.time){
                    this.transform.position = targetP + targetV*(Time.time-targetT);
                    this.transform.eulerAngles = targetR;
                }else{
                    this.transform.position = Vector3.Lerp(this.transform.position, targetP, (Time.time-targetT)* UpdatesPerSecond);
                    this.transform.eulerAngles = Vector3.Lerp(this.transform.eulerAngles, targetR, (Time.time-targetT)* UpdatesPerSecond);
                }
            }
        }
    }
}