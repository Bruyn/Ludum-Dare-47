using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;

    public float gravity = -9.8f;
    public float groundDistance = 0.4f;
    public float speed = 12f;
    public float jumpHeigth = 3f;

    private Vector3 _velocity;
    private bool _isGrounded;

    public Authority _authority;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.DrawLine(Vector3.zero, new Vector3(5, 0, 0), Color.white, 2.5f);
    }

    public void Move(float x, float z, bool jumping)
    {
        if (!_authority.Enabled) return;
        
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * (speed * Time.deltaTime));

        if (jumping && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeigth * -2f * gravity);
        }
        
        _velocity.y += gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);
        
        
        
    }

    private float q = 0.0f;
    
    void FixedUpdate()
    {
        // always draw a 5-unit colored line from the origin
        Color color = new Color(q, q, 1.0f);
        Debug.DrawLine(Vector3.zero, new Vector3(0, 5, 0), color);
        q = q + 0.01f;

        if (q > 1.0f)
        {
            q = 0.0f;
        }
    }
    
    
    public void SetPos(Vector3 posToSet)
    {
        controller.enabled = false;
        transform.position = posToSet;
        controller.enabled = true;
    }
}
