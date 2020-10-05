using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	[SerializeField] private Transform	playerBody;
	[SerializeField] private float		mouseSensitivity = 2f;
    
	public float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Rotate(float axisX, float axisY)
    {
        float mouseX = axisX * mouseSensitivity;
        float mouseY = axisY * mouseSensitivity;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);        
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void Undo(float xRot)
    {
        xRotation = xRot;
    }
    
}
