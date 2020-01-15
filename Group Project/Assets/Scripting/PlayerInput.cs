using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BIAB.Networking;
using Unity.Mathematics;

/// <summary>
/// Takes In PlayerInput to send to server and PlayerMovement
/// This Includes movement, pause states, connection/disconnection calls, and shooting
/// </summary>
public class PlayerInput : MonoBehaviour
{
    private Client client;
    private GameManager gameManager;
    private LocalPlayerMovement movement;
    private void Init()
    {
        client = NetworkManager.main.GetClient();
        if (gameManager.isSinglePlayer())
            movement = this.gameObject.AddComponent<LocalPlayerMovement>();
    }

    private void Update()
    {
        if(gameManager == null)
        {
            gameManager = GameManager.main;
            if(gameManager != null)
            if (gameManager.isSinglePlayer())
                movement = this.gameObject.AddComponent<LocalPlayerMovement>();
            else return;
        }
        float2 move = new float2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (gameManager.isSinglePlayer())
        {
            movement.Move(move, Input.GetAxis("Mouse X"));
            if (Input.GetKeyDown(KeyCode.Space))
            {
                movement.Jump();
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                movement.Shoot();
            }
        }
        else
        {
            client.SendMessage(new PlayerInputNetMsg(move));
            if (Input.GetKeyDown(KeyCode.Space))
            {
                client.SendMessage(new PlayerJumpInputNetMsg());
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                client.SendMessage(new PlayerShootInputNetMsg((float3)this.transform.eulerAngles));
            }
        }
    }
}

