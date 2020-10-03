using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public Rigidbody rb;
    public float speed = 1;
    public float jumptHeight = 5;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 velocity = rb.velocity;
        velocity.x = horizontal * speed;
        velocity.z = vertical * speed;

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            velocity.y += jumptHeight;
        }


        rb.velocity = velocity;
    }

    bool IsGrounded()
    {
        float distToGround = 0.5f;
        return Physics.Raycast(transform.position, -Vector3.up, distToGround);
    }
}