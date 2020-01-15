using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnabledIfDebug : MonoBehaviour
{
    private void Awake()
    {
        if (Debug.isDebugBuild == false && Application.isEditor == false)
            this.gameObject.SetActive(false);
    }
}
