using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LocalPlayerMovement : MonoBehaviour
{
    private Rigidbody Rigid;
    public float JumpForce = 10f;
    public float MouseSensitivity = 10f;
    public float MoveSpeed = 0.2f;
    private void Start()
    {
        Rigid = this.gameObject.AddComponent<Rigidbody>();
        Rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
    public void Move(float2 input, float mouseX)
    {
        if (Rigid != null)
        {
            transform.eulerAngles += new Vector3(0, mouseX * MouseSensitivity, 0);
            Rigid.MovePosition(transform.position + (transform.forward * input.y * MoveSpeed) + (transform.right * input.x * MoveSpeed));
        }  
    }
    public void Jump()
    {
        if (Rigid != null)
        {
            Rigid.AddForce(transform.up * JumpForce);
        }
    }
    public void Shoot()
    {
        throw new System.NotImplementedException("Local Player Movement");
    }
}
