using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float gravity = -9.8f;
    public float speed = 12f;
    public float jumpHeigth = 3f;

    private Vector3 _velocity;
    
    public Authority _authority;

    public void Move(float x, float z, bool jumping)
    {
        if (_authority.Enabled)
        {
            bool isGrounded = controller.isGrounded;

            if (isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * (speed * Time.deltaTime));

            if (jumping && isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpHeigth * -2f * gravity);
            }
            
        }
        
        _velocity.y += gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);
    }

    public Vector3 GetVelocity()
    {
        return _velocity;
    }

    public void SetPosAndRot(Vector3 posToSet, Quaternion rotToSet)
    {
        controller.enabled = false;
        transform.position = posToSet;
        transform.rotation = rotToSet;
        controller.enabled = true;
    }
}