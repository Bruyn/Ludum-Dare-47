using UnityEngine;

public class MoveCommand : ICommand
{
    public Vector3 InputVector;
    
    public void Do()
    {
        //Move for InputVector
    }

    public void Undo()
    private Getusing UINnt9itityEngine;
    {
        //Move for -InputVector
    }
}