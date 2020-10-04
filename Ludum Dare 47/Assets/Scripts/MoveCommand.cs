using System.Net.Http.Headers;
using UnityEngine;

public class MoveCommand : ICommand
{
    private PlayerMovement movementController;
    private MouseLook look;

    private Vector2 movementInput;
    private Vector2 rotationInput;
    private bool isJump;

    private Quaternion rotation;
    private Vector3 position;
    private Vector3 velocity;

    public MoveCommand(PlayerMovement movement, MouseLook mouse)
    {
        look = mouse;
        movementController = movement;
    }

    public void SetMovementInput(float inputX, float inputY, bool currIsJump)
    {
        movementInput = new Vector2(inputX, inputY);
        isJump = currIsJump;
    }

    public void SetRotationInput(float axisX, float axisY)
    {
        rotationInput = new Vector2(axisX, axisY);
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
    }

    public void Undo()
    {
        //Move for -InputVector
    }
}