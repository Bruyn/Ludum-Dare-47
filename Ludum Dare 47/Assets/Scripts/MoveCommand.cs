using System.Net.Http.Headers;
using UnityEngine;

public class MoveCommand : ICommand
{
    private PlayerMovement movementController;
    private MouseLook look;
    private GameObject gameObject;
    private GameObject camera;

    private Vector2 movementInput;
    private Vector2 rotationInput;
    private bool isJump;

    private Quaternion rotation;
    private float cameraY;
    private Vector3 position;
    private Vector3 velocity;

    public MoveCommand(PlayerMovement movement, MouseLook mouse, GameObject obj, GameObject cam)
    {
        look = mouse;
        movementController = movement;
        gameObject = obj;
        camera = cam;
    }

    public void SetMovementInput(float inputX, float inputY, bool currIsJump)
    {
        movementInput = new Vector2(inputX, inputY);
        isJump = currIsJump;
    }

    public void SetRotationInput(float axisX, float axisY, float camY)
    {
        rotationInput = new Vector2(axisX, axisY);
        cameraY = camY;
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
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        
        var currRot = camera.transform.localRotation;
        currRot.x = cameraY;
        camera.transform.localRotation = currRot;
    }
}