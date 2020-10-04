using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement movement;

    [SerializeField] private MouseLook look;

    [SerializeField]
    private List<ICommand> commands = new List<ICommand>();

    [SerializeField] private bool executingCommands = false;
    private Vector3 rewindPosition;
    private Quaternion rewindRotation;

    [SerializeField] KeyCode recordToggle;
    [SerializeField] private KeyCode playBackKey;

    [SerializeField] private Color recordedPointsColor;
    [SerializeField] private Color playedBackPointsColor;

    private Text recordText;
    private bool isRecordingNow = false;

    private bool isPlayingBack = false;
    private float mouseX = .0f;
    private float mouseY = .0f;

    private List<Vector3> recordedPoints = new List<Vector3>();
    private List<Vector3> playedBackPoints = new List<Vector3>();
    
    private void Awake()
    {
        var obj = GameObject.FindWithTag("recordText");
        recordText = obj.GetComponent<Text>();

        recordText.text = "Not recording";
    }

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        QualitySettings.vSyncCount = 0;
    }
    
    private void Update()
    {
        mouseX += Input.GetAxis("Mouse X");
        mouseY += Input.GetAxis("Mouse Y");
    }

    void FixedUpdate()
    {
        if (Application.targetFrameRate != 60)
            Application.targetFrameRate = 60;
        
        if (executingCommands)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool jump = Input.GetButtonDown("Jump");

        if (Input.GetKeyDown(recordToggle) && !executingCommands)
        {
            isRecordingNow = !isRecordingNow;
            recordText.text = isRecordingNow ? "Recording" : "Not Recording";

            if (isRecordingNow)
            {
                recordedPoints.Clear();
                playedBackPoints.Clear();
                
                commands.Clear();
                rewindPosition = transform.position;
                rewindRotation = transform.rotation;
                lel = 0;
            }
        }

        if (!executingCommands)
        {
            look.Rotate(mouseX, mouseY);
            if (!isRecordingNow)
            {
                mouseX = .0f;
                mouseY = .0f;
            }
            
            movement.Move(horizontal, vertical, jump);
        }
        
        if (isRecordingNow)
        {
            AddMoveCommand(horizontal, vertical, jump, mouseX, mouseY);
            mouseX = .0f;
            mouseY = .0f;

            recordedPoints.Add(transform.position);
            return;
        }

        if (Input.GetKey(playBackKey))
        {
            if (commands.Count <= lel)
            {
                return;
            }
            
            if (lel == 0)
                movement.SetPosAndRot(rewindPosition, rewindRotation);
            
            commands[lel].Do();
            playedBackPoints.Add(transform.position);
            lel++;

            recordText.text = "Playing Back";
            // StartCoroutine(ExecuteCommands());
        }
    }

    private int lel = 0;

    private void OnDrawGizmos()
    {
        if (recordedPoints.Count > 1)
        {
            for (int i = 0; i < recordedPoints.Count - 1; ++i)
            {
                Gizmos.color = recordedPointsColor;
                Gizmos.DrawLine(recordedPoints[i], recordedPoints[i+1]);
            }
        }
        
        if (playedBackPoints.Count > 1)
        {
            for (int i = 0; i < playedBackPoints.Count - 1; ++i)
            {
                Gizmos.color = playedBackPointsColor;
                Gizmos.DrawLine(playedBackPoints[i], playedBackPoints[i+1]);
            }
        }
    }

    void AddMoveCommand(float x, float y, bool jump, float axisX, float axisY)
    {
        MoveCommand moveCommand = new MoveCommand(movement, look);
        moveCommand.SetPosition(x, y, jump);
        moveCommand.SetRotation(axisX, axisY);
        
        commands.Add(moveCommand);
    }

    IEnumerator ExecuteCommands()
    {
        Debug.Log("Commands executing. Total commands: " + commands.Count); ;
        movement.SetPosAndRot(rewindPosition, rewindRotation);
        executingCommands = true;
        List<ICommand> commandsCopy = new List<ICommand>(commands);
        foreach (var command in commandsCopy)
        {
            command.Do();
            playedBackPoints.Add(transform.position);
            yield return null;
        }
        yield return null;
        commands.Clear();
        executingCommands = false;
        recordText.text = "Not Recocrding";
        Debug.Log("Commands executed. Total commands: " + commands.Count); ;
    }
}