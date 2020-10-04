using UnityEngine;

public class MoveCommand : ICommand
{
    public PlayerMovement Controller;
    public MouseLook look;
    public Vector2 InputVector;
    public Vector2 RotationVector;
    public bool Jump;

    public MoveCommand(PlayerMovement controller, MouseLook mouseLook)
    {
        look = mouseLook;
        Controller = controller;
    }

    public void SetPosition(float inputX, float inputY, bool isJump)
    {
        Jump = isJump;
        InputVector = new Vector2(inputX, inputY);
    }
    
    public void SetRotation(float axisX, float axisY)
    {
        RotationVector = new Vector2(axisX, axisY);
    }

public void Do()
    {
        look.Rotate(RotationVector.x, RotationVector.y);
        Controller.Move(InputVector.x, InputVector.y, Jump);
    }

    public void Undo()
    {
        //Move for -InputVector
    }
}