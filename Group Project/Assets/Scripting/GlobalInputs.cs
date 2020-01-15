using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInputs : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F11)){
            Screen.fullScreen = !Screen.fullScreen;
            if(Screen.fullScreen)
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            }else{
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
            }
        }
            
    }
}
