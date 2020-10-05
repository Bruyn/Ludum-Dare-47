using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlaybackMode
{
    Pause,
    PlayAndRecord,
    Rewind,
    FastForward
}

public class SimulationController : MonoBehaviour
{
    public static SimulationController Instance;
    
    public KeyCode pauseToggleKey;
    public KeyCode rewindKey;
    public KeyCode forwardKey;
    public KeyCode fastPlayBackKey;

    public string simulatedObjectTag;

    private PlaybackMode currentMode = PlaybackMode.PlayAndRecord;
    private bool isFastPlayBack = false;

    private List<SimulatedEntityBase> simulatedEntities = new List<SimulatedEntityBase>();

    public PlaybackMode GetCurrentMode()
    {
        return currentMode;
    }

    public bool IsFastPlayBack()
    {
        return isFastPlayBack;
    }

    private void Awake()
    {
        Instance = this;
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
                case PlaybackMode.Pause:
                    currentMode = PlaybackMode.PlayAndRecord;
                    break;
                case PlaybackMode.PlayAndRecord:
                    currentMode = PlaybackMode.Pause;
                    break;
            }
        }

        if (currentMode != PlaybackMode.PlayAndRecord)
        {
            ProcessKeys();
        }
    }

    private void ProcessKeys()
    {
        if (Input.GetKey(rewindKey))
        {
            currentMode = PlaybackMode.Rewind;
        }

        if (Input.GetKey(forwardKey))
        {
            currentMode = PlaybackMode.FastForward;
        }

        if (Input.GetKeyUp(rewindKey) || Input.GetKeyUp(forwardKey))
        {
            currentMode = PlaybackMode.Pause;
        }

        isFastPlayBack = Input.GetKey(fastPlayBackKey);
    }

    private void FixedUpdate()
    {
        if (isFastPlayBack)
        {
            Time.timeScale = 2;
        }
        else
        {
            Time.timeScale = 1;
        }
        
        foreach (var entity in simulatedEntities)
        {
            entity.TriggerSimulate(currentMode);
        }
    }
}