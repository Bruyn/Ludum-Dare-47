using UnityEngine;

public class MoveCommand : ICommand
{
    public Vector3 InputVector;
    
    public void Do()
    {
        //Move for InputVector
    }

    public void Undo()
    {
        //Move for -InputVector
    }
}