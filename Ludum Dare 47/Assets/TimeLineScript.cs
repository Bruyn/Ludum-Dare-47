using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeLineScript : MonoBehaviour
{
    public Text timeTextBox;
    public Slider progressSlider;
    
    private SimulationController _simulationController;

    void Start()
    {
        var mngObject = GameObject.FindWithTag("simulationController");
        if (mngObject != null)
        {
            _simulationController = mngObject.GetComponent<SimulationController>();
        }
    }

    void Update()
    {
        if (_simulationController == null)
            return;

        var currTimeIn = _simulationController.GetCurrentTimeIn();
        TimeSpan timeSpan = TimeSpan.FromSeconds(currTimeIn);
        
        progressSlider.value = _simulationController.GetSimulationProgress();
        timeTextBox.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
    }
}