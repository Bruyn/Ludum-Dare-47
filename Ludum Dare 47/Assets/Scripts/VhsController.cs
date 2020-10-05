using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VhsController : MonoBehaviour
{
    private SimulationController simulationController;
    [SerializeField]
    private VideoPlayer videoPlayer;
    [SerializeField]
    private VHSPostProcessEffect glitchEffect;

    private void Start()
    {
        simulationController = SimulationController.Instance;
    }

    private void Update()
    {
        PlaybackMode mode = simulationController.GetCurrentMode();

        if (mode == PlaybackMode.Pause)
        {
            videoPlayer.enabled = true;
            glitchEffect.enabled = true;
            videoPlayer.playbackSpeed = 0;
        }
        else if (mode == PlaybackMode.Rewind || mode == PlaybackMode.FastForward)
        {
            videoPlayer.enabled = true;
            glitchEffect.enabled = true;
            videoPlayer.playbackSpeed = 10;
        }
        else
        {
            videoPlayer.enabled = false;
            glitchEffect.enabled = false;
        }

    }
}
