using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
	[SerializeField] private Authority	authority;
	[SerializeField] private Transform	playerBody;
	[SerializeField] private float		mouseSensitivity = 2f;

	private float xRotation = 0f;
	private bool cursorLocked = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.lockState = cursorLocked ? CursorLockMode.None : CursorLockMode.Locked;
			Cursor.visible = !cursorLocked;
			cursorLocked = !cursorLocked;
		}
	}

    public void Rotate(float axisX, float axisY)
    {
        if (!authority.Enabled) return;
        if (!cursorLocked)		return;

        float mouseX = axisX * mouseSensitivity;
        float mouseY = axisY * mouseSensitivity;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
