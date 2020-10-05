using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBox : MonoBehaviour
{
    public Rigidbody rb;
    
    void Update()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        
        if (Input.GetKeyDown("u"))
        {
            //Ray direction = cam.ScreenPointToRay(Input.mousePosition);
            rb.transform.position = ray.origin + (ray.direction * 2);
            rb.transform.forward = ray.direction;
            rb.velocity = ray.direction * 10;
        }
    }
}
