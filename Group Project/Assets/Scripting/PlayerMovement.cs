using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Takes in Update Positions and lerps to them for moving objects (Ment for players)
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public bool canMove = true;

    public void Init(bool canMove = true)
    {
        this.canMove = canMove;
    }

    public void NewPosition(Vector3 position)
    {
        if(canMove)
            this.transform.position = position;
    }
}
