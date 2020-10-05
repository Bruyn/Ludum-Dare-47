using System.Net.Http.Headers;
using UnityEngine;

public class MoveCommand : ICommand
{
    private PlayerMovement movementController;
    private ObjectInteraction _interaction;
    private MouseLook look;
    private GameObject gameObject;
    private GameObject camera;

    private Vector2 movementInput;
    private Vector2 rotationInput;
    private bool isJump;
    public bool isInteracted;

    private Quaternion rotation;
    private Quaternion cameraRotation;
    private Vector3 position;
    public Vector3 velocity;
    private float _xRotation;

    public MoveCommand(PlayerMovement movement, MouseLook mouse, GameObject obj, GameObject cam, ObjectInteraction interaction)
    {
        look = mouse;
        movementController = movement;
        gameObject = obj;
        camera = cam;
        _interaction = interaction;
    }

    public void SetMovementInput(float inputX, float inputY, bool currIsJump, bool inter)
    {
        movementInput = new Vector2(inputX, inputY);
        isJump = currIsJump;
        isInteracted = inter;
    }

    public void SetRotationInput(float axisX, float axisY, Quaternion camRot)
    {
        rotationInput = new Vector2(axisX, axisY);
        cameraRotation = camRot;
        _xRotation = look.xRotation;
    }

    public void SetTransform(Quaternion currRotation, Vector3 currPosition)
    {
        rotation = currRotation;
        position = currPosition;
    }

    public void SetVelocity(Vector3 currVelocity)
    {
        velocity = currVelocity;
    }

    public Vector3 GetPos()
    {
        return position;
    }

    public Quaternion GetRot()
    {
        return rotation;
    }

    public void Do()
    {
        look.Rotate(rotationInput.x, rotationInput.y);
        movementController.Move(movementInput.x, movementInput.y, isJump);
        
        if (isInteracted)
            _interaction.Interact();
    }

    public void Undo()
    {
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        camera.transform.localRotation = cameraRotation;
        look.Undo(_xRotation);
        
        if (isInteracted)
            _interaction.Interact();
    }
}