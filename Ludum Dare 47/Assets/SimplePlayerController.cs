using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float gravity = -9.8f;
    public float speed = 12f;
    public float jumpHeigth = 3f;

    private Vector3 _velocity;

    public Camera Camera;

    public float mouseSensitivity = 2f;

    public Transform playerBody;

    private float _xRotation = 0f;
    private float _mouseY = 0f;
    private float _mouseX = 0f;

    private float _xAxis = 0f;
    private float _zAxis = 0f;
    private bool _jumping = false;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFixed)
        {
            Simulate();
            Debug.Log("Timedelta" + Time.deltaTime);
        }
    }

    private bool isFixed = false;

    private void Update()
    {
        //Debug.Log("Timedelta" + Time.deltaTime);
        _mouseX += Input.GetAxis("Mouse X") * mouseSensitivity;
        _mouseY += Input.GetAxis("Mouse Y") * mouseSensitivity;

        _xAxis += Input.GetAxis("Horizontal");
        _zAxis += Input.GetAxis("Vertical");
        _jumping = Input.GetButtonDown("Jump");

        Time.timeScale = 1;

        if (Input.GetKeyDown("p"))
        {
            isFixed = !isFixed;
            Debug.Log("isFixed " + isFixed + "Timedelta" + Time.deltaTime);
        }

        if (!isFixed)
        {
            Simulate();
        }
    }

    void Simulate()
    {
        _xRotation -= _mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        Camera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * _mouseX);

        _mouseX = .0f;
        _mouseY = .0f;

        bool isGrounded = controller.isGrounded;

        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }


        Vector3 move = transform.right * _xAxis + transform.forward * _zAxis;
        controller.Move(move * (speed * Time.deltaTime));

        if (_jumping && isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeigth * -2f * gravity);
        }

        _jumping = false;
        _xAxis = 0f;
        _zAxis = 0f;
        
        _velocity.y += gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);
    }
}