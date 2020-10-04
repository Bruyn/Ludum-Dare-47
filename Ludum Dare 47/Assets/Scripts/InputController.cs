using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public PlayerMovement movementControl;
    public MouseLook mouseControl;
    public GameObject cameraObject;
    public Authority authority;
    public Color trailColor;

    private List<ICommand> commands = new List<ICommand>();
    private int lastComandIdx = -1;

    private float mouseX = .0f;
    private float mouseY = .0f;
    private float horizontalAxis = .0f;
    private float verticalAxis = .0f;
    private bool isJump = false;

    private void Update()
    {
        if (authority.Enabled)
        {
            mouseX += Input.GetAxis("Mouse X");
            mouseY += Input.GetAxis("Mouse Y");
            horizontalAxis = Input.GetAxis("Horizontal");
            verticalAxis = Input.GetAxis("Vertical");
            isJump = Input.GetButton("Jump") || isJump;
        }
    }

    private void OnDrawGizmos()
    {
        if (commands.Count > 1)
        {
            for (int i = 0; i < commands.Count - 1; ++i)
            {
                var currCommand = (MoveCommand) commands[i];
                var nextCommand = (MoveCommand) commands[i + 1];
                if (currCommand == null || nextCommand == null)
                {
                    continue;
                }

                Gizmos.color = trailColor;
                Gizmos.DrawLine(currCommand.GetPos(), nextCommand.GetPos());
            }
        }
    }

    public void Simulate(PLaybackMode mode)
    {
        switch (mode)
        {
            case PLaybackMode.Pause:
                movementControl._velocity = ((MoveCommand) commands[lastComandIdx]).velocity;
                break;
            case PLaybackMode.PlayAndRecord:
                if (authority.Enabled && lastComandIdx > 0 && lastComandIdx < commands.Count - 1)
                {
                    commands.RemoveRange(lastComandIdx, commands.Count - lastComandIdx);
                }

                if (commands.Count == 0 || lastComandIdx >= commands.Count - 1)
                {
                    var command = AddMoveCommand();
                    command.Do();
                }
                else
                {
                    TryDoCommand();
                }

                break;
            case PLaybackMode.FastForward:
                TryDoCommand();
                break;
            case PLaybackMode.Rewind:
                TryUndoCommand();
                break;
        }

        ResetInput();
    }

    private void ResetInput()
    {
        mouseX = .0f;
        mouseY = .0f;
        isJump = false;
    }

    private MoveCommand AddMoveCommand()
    {
        MoveCommand moveCommand = new MoveCommand(movementControl, mouseControl, gameObject, cameraObject);
        moveCommand.SetMovementInput(horizontalAxis, verticalAxis, isJump);
        moveCommand.SetRotationInput(mouseX, mouseY, cameraObject.transform.localRotation);
        moveCommand.SetTransform(transform.rotation, transform.position);
        moveCommand.SetVelocity(movementControl.GetVelocity());

        commands.Add(moveCommand);
        lastComandIdx = commands.Count - 1;

        return moveCommand;
    }

    private void TryDoCommand()
    {
        if (lastComandIdx >= commands.Count - 1)
            return;

        lastComandIdx++;
        commands[lastComandIdx].Do();
    }

    private void TryUndoCommand()
    {
        if (lastComandIdx <= 0)
            return;

        lastComandIdx--;
        commands[lastComandIdx].Undo();
    }
}