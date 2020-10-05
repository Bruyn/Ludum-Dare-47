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
    public ObjectInteraction objectInteraction;
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
    private bool isInteract = false;

    private void Update()
    {
        if (authority.Enabled)
        {
            mouseX += Input.GetAxis("Mouse X");
            mouseY += Input.GetAxis("Mouse Y");
            horizontalAxis = Input.GetAxis("Horizontal");
            verticalAxis = Input.GetAxis("Vertical");
            isJump = Input.GetButton("Jump") || isJump;
            isInteract = Input.GetButtonDown("Fire3") || isInteract;
        }
    }

    private void OnDrawGizmos()
    {
        if (commands.Count > 1)
        {
            for (int i = 0; i < commands.Count - 1; ++i)
            {
                var currCommand = commands[i] as MoveCommand;
                var nextCommand = commands[i + 1] as MoveCommand;
                if (currCommand == null || nextCommand == null)
                {
                    continue;
                }

                Gizmos.color = trailColor;
                Gizmos.DrawLine(currCommand.GetPos(), nextCommand.GetPos());
            }
        }
    }

    public int GetCurrentCommandIdx()
    {
        return lastComandIdx;
    }

    public void Simulate(PlaybackMode mode)
    {
        switch (mode)
        {
            case PlaybackMode.Pause:
                movementControl._velocity = ((MoveCommand) commands[lastComandIdx]).velocity;
                break;
            case PlaybackMode.PlayAndRecord:
                if (authority.Enabled && lastComandIdx > 0 && lastComandIdx < commands.Count - 1)
                {
                    commands.RemoveRange(lastComandIdx, commands.Count - lastComandIdx);
                }

                if (commands.Count == 0 || lastComandIdx >= commands.Count - 1)
                {
                    var moveCommand = AddMoveCommand();
                    moveCommand.Do();
                }
                else
                {
                    TryDoCommand();
                }

                break;
            case PlaybackMode.FastForward:
                TryDoCommand();
                break;
            case PlaybackMode.Rewind:
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
        isInteract = false;
        horizontalAxis = 0f;
        verticalAxis = 0f;
    }

    private MoveCommand AddMoveCommand()
    {
        MoveCommand moveCommand = new MoveCommand(movementControl, mouseControl, gameObject, cameraObject, objectInteraction);
        moveCommand.SetMovementInput(horizontalAxis, verticalAxis, isJump, isInteract);
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