using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeToMainMenu : MonoBehaviour
{
    public bool ShowText = false;
    public Vector2 position = Vector2.zero;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
    private void OnGUI()
    {
        if(ShowText)
            GUI.Label(new Rect(10+position.x, 10 + position.y, 250, 20), "Escape To Exit To Main Menu");
    }
}
