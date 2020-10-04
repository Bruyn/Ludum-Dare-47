using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 2f;

    public Transform playerBody;

    private float xRotation = 0f;
    
    public Authority _authority;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Rotate(float axisX, float axisY)
    {
        if (!_authority.Enabled) return;
        
        float mouseX = axisX * mouseSensitivity;
        float mouseY = axisY * mouseSensitivity;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
