using UnityEngine;

public class MoveCommand : ICommand
{
    public PlayerController Controller;
    public Vector2 InputVector;
    public bool Jump;

    public MoveCommand(PlayerController controller, float x, float y, bool jump)
    {
        InputVector = new Vector2(x, y);
        Jump = jump;
        Controller = controller;
    }
    
    public void Do()
    {
        Controller.Move(InputVector.x, InputVector.y, Jump);
    }

    public void Undo()
    {
        //Move for -InputVector
    }
}