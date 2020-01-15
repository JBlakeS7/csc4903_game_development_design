using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BIAB.Networking
{
    [Obsolete("Not Implemented!")]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class NetworkedAttribute : Attribute
    {
        private string _ID;
        public string ID
        {
            get
            {
                return _ID;
            }
        }

        public NetworkedAttribute( string ID)
        {
            _ID = ID;
        }
    }
}