using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Rigidbody rb;

    public Camera cam;
    // Update is called once per frame
    void Update()
    {
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
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
