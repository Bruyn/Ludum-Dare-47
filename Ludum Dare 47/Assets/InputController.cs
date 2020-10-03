using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private PlayerController controller;

    [SerializeField]
    private List<ICommand> commands = new List<ICommand>();

    [SerializeField] private bool executingCommands = false;
    private Vector3 rewindPosition;
    
    void Start()
    {
        controller = GetComponent<PlayerController>();
        QualitySettings.vSyncCount = 0;
        rewindPosition = transform.position;
    }
    
    void Update()
    {
        if (Application.targetFrameRate != 60)
            Application.targetFrameRate = 60;
        
        if (executingCommands)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool jump = Input.GetButtonDown("Jump");
        AddMoveCommand(horizontal, vertical, jump);
        
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(ExecuteCommands());
        }
    }

    void AddMoveCommand(float x, float y, bool jump)
    {
        MoveCommand moveCommand = new MoveCommand(controller, x, y, jump);
        commands.Add(moveCommand);
        moveCommand.Do();
        Debug.Log("Command added. Total commands: " + commands.Count);
    }

    IEnumerator ExecuteCommands()
    {
        transform.position = rewindPosition;
        executingCommands = true;
        List<ICommand> commandsCopy = new List<ICommand>(commands);
        foreach (var command in commandsCopy)
        {
            command.Do();
            yield return null;
        }
        yield return null;
        commands.Clear();
        executingCommands = false;
        rewindPosition = transform.position;
        Debug.Log("Commands executed. Total commands: " + commands.Count); ;
    }
}