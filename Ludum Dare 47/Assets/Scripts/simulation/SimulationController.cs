using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLaybackMode
{
    Pause,
    PlayAndRecord,
    Rewind,
    FastForward
}

public class SimulationController : MonoBehaviour
{
    public KeyCode pauseToggleKey;
    public KeyCode rewindKey;
    public KeyCode fastForwardKey;

    public string simulatedObjectTag;

    private PLaybackMode currentMode = PLaybackMode.PlayAndRecord;

    private List<SimulatedEntityBase> simulatedEntities = new List<SimulatedEntityBase>();

    public PLaybackMode GetCurrentMode()
    {
        return currentMode;
    }

    void Start()
    {
        if (Application.targetFrameRate != 60)
            Application.targetFrameRate = 60;
        
        var simulatedObjects = GameObject.FindGameObjectsWithTag(simulatedObjectTag);
        foreach (var obj in simulatedObjects)
        {
            simulatedEntities.Add(obj.GetComponent<SimulatedEntityBase>());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseToggleKey))
        {
            switch (currentMode)
            {
                case PLaybackMode.Pause:
                    currentMode = PLaybackMode.PlayAndRecord;
                    break;
                case PLaybackMode.PlayAndRecord:
                    currentMode = PLaybackMode.Pause;
                    break;
            }
        }

        if (currentMode != PLaybackMode.PlayAndRecord)
        {
            ProcessKeys();
        }
    }

    private void ProcessKeys()
    {
        if (Input.GetKey(rewindKey))
        {
            currentMode = PLaybackMode.Rewind;
        }

        if (Input.GetKey(fastForwardKey))
        {
            currentMode = PLaybackMode.FastForward;
        }

        if (Input.GetKeyUp(rewindKey) || Input.GetKeyUp(fastForwardKey))
        {
            currentMode = PLaybackMode.Pause;
        }
    }

    private void FixedUpdate()
    {
        foreach (var entity in simulatedEntities)
        {
            entity.TriggerSimulate(currentMode);
        }
    }
}