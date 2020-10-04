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
    public Authority authority;

    public Color recordedPointsColor;
    public Color playedBackPointsColor;

    private bool executingCommands = false;
    private List<ICommand> commands = new List<ICommand>();
    private int lastComandIdx = -1;
    private Vector3 rewindPosition;
    private Quaternion rewindRotation;

    private List<Vector3> recordedPoints = new List<Vector3>();
    private List<Vector3> playedBackPoints = new List<Vector3>();

    private float mouseX = .0f;
    private float mouseY = .0f;
    private float horizontalAxis = .0f;
    private float verticalAxis = .0f;
    private bool isJump = false;

    void Start()
    {
        if (Application.targetFrameRate != 60)
            Application.targetFrameRate = 60;

        movementControl = GetComponent<PlayerMovement>();
        QualitySettings.vSyncCount = 0;
    }

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

    public void Simulate(PLaybackMode mode)
    {
        switch (mode)
        {
            case PLaybackMode.PlayAndRecord:
                if (authority.Enabled)
                {
                    ApplyMoveAndRotate();
                    AddMoveCommand();
                }
                else
                {
                    TryExecuteCommand();
                }

                break;
            case PLaybackMode.FastForward:
                TryExecuteCommand();
                break;
            case PLaybackMode.Rewind:
                TryRestoreCommand();
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

    private void OnDrawGizmos()
    {
        if (recordedPoints.Count > 1)
        {
            for (int i = 0; i < recordedPoints.Count - 1; ++i)
            {
                Gizmos.color = recordedPointsColor;
                Gizmos.DrawLine(recordedPoints[i], recordedPoints[i + 1]);
            }
        }

        if (playedBackPoints.Count > 1)
        {
            for (int i = 0; i < playedBackPoints.Count - 1; ++i)
            {
                Gizmos.color = playedBackPointsColor;
                Gizmos.DrawLine(playedBackPoints[i], playedBackPoints[i + 1]);
            }
        }
    }

    private void AddMoveCommand()
    {
        if (lastComandIdx < commands.Count - 1)
        {
            commands.RemoveRange(lastComandIdx, commands.Count - lastComandIdx);
        }

        MoveCommand moveCommand = new MoveCommand(movementControl, mouseControl);
        moveCommand.SetMovementInput(horizontalAxis, verticalAxis, isJump);
        moveCommand.SetRotationInput(mouseX, mouseY);
        moveCommand.SetTransform(transform.rotation, transform.position);
        moveCommand.SetVelocity(movementControl.GetVelocity());

        commands.Add(moveCommand);
        lastComandIdx = commands.Count - 1;
    }

    private void TryExecuteCommand()
    {
        if (lastComandIdx == commands.Count - 1)
            return;

        commands[lastComandIdx].Do();
        lastComandIdx++;
    }

    private void ApplyMoveAndRotate()
    {
        mouseControl.Rotate(mouseX, mouseY);
        movementControl.Move(horizontalAxis, verticalAxis, isJump);
    }

    private void TryRestoreCommand()
    {
        if (lastComandIdx == 0)
            return;

        var currCommand = (MoveCommand) commands[lastComandIdx];

        transform.position = currCommand.GetPos();
        transform.rotation = currCommand.GetRot();

        lastComandIdx--;
    }
}