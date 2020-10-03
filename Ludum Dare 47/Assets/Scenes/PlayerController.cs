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

    public void Move(float x, float y)
    {
        Vector3 velocity = rb.velocity;
        velocity.x = x * speed * Time.deltaTime;
        velocity.z = y * speed * Time.deltaTime;

        rb.velocity = velocity;
    }

    public void Jump()
    {
        Vector3 velocity = rb.velocity;
        if (IsGrounded())
            velocity.y += jumptHeight;

        rb.velocity = velocity;
    }

    bool IsGrounded()
    {
        float distToGround = 0.5f;
        return Physics.Raycast(transform.position, -Vector3.up, distToGround);
    }
}